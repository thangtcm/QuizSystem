using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Services;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduQuiz_5P.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Roles = Constants.Roles.Admin)]
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly IUserService _userService;
        private readonly IExamMatrixService _examMatrix;
        private readonly IQuestionService _questionService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;
        public ExamController(IExamService examService, IUserService userService,
            IExamMatrixService examMatrix, IQuestionService questionService, IClassService classService, ISubjectService subjectService)
        {
            _examService = examService;
            _userService = userService;
            _examMatrix = examMatrix;
            _questionService = questionService;
            _classService = classService;
            _subjectService = subjectService;
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _examService.GetListAsync();
            return View(applicationDbContext);
        }

        public async Task<IActionResult> Create()
        {
            var examMatrix = await _examMatrix.GetListAsync();
            ViewData["ExamMatrix"] = new SelectList(examMatrix, "Id", "ExamMatrixName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam exam, int examMatrixId)
        {
            var examMatrix = await _examMatrix.GetListAsync();
            if (examMatrixId == 0)
            {
                ModelState.AddModelError(string.Empty, "Bạn chưa chọn ma trận đề");
                ViewData["ExamMatrix"] = new SelectList(examMatrix, "Id", "ExamMatrixName");
                return View();
            }
            try
            {
                await _examService.CreateExamWithMatrix(exam, examMatrixId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra");
            }
            ViewData["ExamMatrix"] = new SelectList(examMatrix, "Id", "ExamMatrixName");
            return View(exam);
        }

        public async Task<IActionResult> CreateFile()
        {
            var classlist = await _classService.GetListAsync();
            var subjectlist = await _subjectService.GetListAsync();
            ViewData["SelectListClass"] = new SelectList(classlist, "Id", "ClassName");
            ViewData["SelectListSubject"] = new SelectList(subjectlist, "Id", "SubjectName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFile(ImportExamFileVM model)
        {
            
            try
            {
                await _examService.CreateExamImport(model);
                this.AddToastrMessage("Tạo bài thi thành công.", ToastrMessageType.Success);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra");
            }
            var classlist = await _classService.GetListAsync();
            ViewData["SelectListClass"] = new SelectList(classlist, "Id", "ClassName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DynamicImportQuestion(IFormFile fileUpload, string ExamName, string ExamDescription, int ExamTime, int SubjectId, int ClassId, ExamType ExamType)
        {
            ImportExamFileVM model = new();
            try
            {
                model.ExamName = ExamName;
                model.ExamDescription = ExamDescription;
                model.ExamTime = ExamTime;
                model.ExamSubjectId = SubjectId;
                model.ExamClassId = ClassId;
                model.ExamType = ExamType;
                if (Path.GetExtension(fileUpload.FileName) == ".tex")
                {
                    var question = await _questionService.ReadFileLatex(fileUpload);
                    model.QuestionVMs = question.ToList();
                    return PartialView("_DynamicPreview", model);
                }
                else if (Path.GetExtension(fileUpload.FileName) == ".docx")
                {
                    var question = (await _questionService.ReadFileDoc(fileUpload)) ?? new List<QuestionVM>();
                    model.QuestionVMs = question.ToList();
                    return PartialView("_DynamicPreview", model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi {ex.Message}");
            }
            return NotFound();
        }
    }
}
