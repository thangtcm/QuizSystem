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
            var subjectLst = await _subjectService.GetListAsync();
            ViewData["SubjectList"] = new SelectList(subjectLst, "Id", "SubjectName");
            var applicationDbContext = await _examService.GetListAsync();
            return View(applicationDbContext);
        }

        public async Task<IActionResult> Create()
        {
            var classlist = await _classService.GetListAsync();
            var subjectlist = await _subjectService.GetListAsync();
            ViewData["SelectListClass"] = new SelectList(classlist, "Id", "ClassName");
            ViewData["SelectListSubject"] = new SelectList(subjectlist, "Id", "SubjectName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam exam, int examMatrixId)
        {
            ICollection<ExamMatrix> examMatrixs = new List<ExamMatrix>();
            try
            {
                var user = await _userService.GetUser();
                examMatrixs = await _examMatrix.GetListAsync();

                if (examMatrixId == 0)
                {
                    ModelState.AddModelError(string.Empty, "Bạn chưa chọn ma trận đề");
                    ViewData["ExamMatrix"] = new SelectList(examMatrixs, "Id", "ExamMatrixName");
                    return View();
                }
                if(user == null)
                {
                    this.AddToastrMessage("Vui lòng đăng nhập lại", ToastrMessageType.Error);
                    return RedirectToAction(nameof(Index));
                }
                await _examService.CreateExamWithMatrix(exam, examMatrixId, user.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra");
            }
            ViewData["ExamMatrix"] = new SelectList(examMatrixs, "Id", "ExamMatrixName");
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
        public async Task<IActionResult> DynamicImportQuestion(ImportExamFileVM model)
        {
            try
            {
                if(model.FileUpload != null)
                {
                    if (Path.GetExtension(model.FileUpload.FileName) == ".tex")
                    {
                        var question = await _questionService.ReadFileLatex(model.FileUpload);
                        model.QuestionVMs = question.ToList();
                        model.ExamIdentification = (model.ExamIdentification / question.Count) * 100;
                        model.ExamUnderstanding = (model.ExamUnderstanding / question.Count) * 100;
                        model.ExamApplication = (model.ExamApplication / question.Count) * 100;
                        model.ExamAdvancedApplication = (model.ExamAdvancedApplication / question.Count) * 100;
                        return PartialView("_DynamicPreview", model);
                    }
                    else if (Path.GetExtension(model.FileUpload.FileName) == ".docx")
                    {
                        var question = (await _questionService.ReadFileDoc(model.FileUpload)) ?? new List<QuestionVM>();
                        model.QuestionVMs = question.ToList();
                        model.ExamIdentification = (model.ExamIdentification / question.Count) * 100;
                        model.ExamUnderstanding = (model.ExamUnderstanding / question.Count) * 100;
                        model.ExamApplication = (model.ExamApplication / question.Count) * 100;
                        model.ExamAdvancedApplication = (model.ExamAdvancedApplication / question.Count) * 100;
                        return PartialView("_DynamicPreview", model);
                    }
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
