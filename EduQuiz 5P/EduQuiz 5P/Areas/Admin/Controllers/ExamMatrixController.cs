using DocumentFormat.OpenXml.VariantTypes;
using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Services;
using EduQuiz_5P.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduQuiz_5P.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Roles = Constants.Roles.Admin)]
    public class ExamMatrixController : Controller
    {
        private readonly ILogger<ExamMatrixController> _logger;
        private readonly IExamMatrixService _examMatrixService;
        private readonly IClassService _classService;
        private readonly IUserService _userService;
        private readonly ISubjectService _subjectService;
        public ExamMatrixController(ILogger<ExamMatrixController> logger, IExamMatrixService examMatrixService, IUserService userService,
            IClassService classService, ISubjectService subjectService)
        { 
            _logger = logger; 
            _examMatrixService = examMatrixService;
            _userService = userService;
            _classService = classService;
            _subjectService = subjectService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _examMatrixService.GetListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var model = new ExamMatrix();
            var classlst = await _classService.GetListAsync();
            var subjectlst = await _subjectService.GetListAsync();
            model.ExamMatrixDetail = new List<ExamMatrixDetail>() { new() {
                SelectListClass = new SelectList(classlst, "Id", "ClassName"),
                SelectListSubject = new SelectList(subjectlst, "Id", "SubjectName")
                } 
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamMatrix model)
        {
            try
            {
                var user = await _userService.GetUser();
                if (user == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                await _examMatrixService.Add(model, user.Id);
                this.AddToastrMessage("Tạo ma trận đề thi thành công", ToastrMessageType.Success);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                this.AddToastrMessage("Đã có lỗi xảy ra", ToastrMessageType.Error);

            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(!id.HasValue)
            {
                this.AddToastrMessage("Đã có lỗi xảy ra", ToastrMessageType.Error);
                return RedirectToAction(nameof(Index));
            }    
            var model = await _examMatrixService.GetByIdAsync(id.Value, x => x.Include(x => x.ExamMatrixDetail!));
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExamMatrix model)
        {
            try
            {
                await _examMatrixService.Update(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Khi cập nhật Một Class : \n" + ex.ToString() + "\n\n");
            }
            return View(model);
        }

        public async Task<IActionResult> CreatePartial()
        {
            var model = new ExamMatrix();
            var classlst = await _classService.GetListAsync();
            var subjectlst = await _subjectService.GetListAsync();
            model.ExamMatrixDetail = new List<ExamMatrixDetail>() { new() {
                SelectListClass = new SelectList(classlst, "Id", "ClassName"),
                SelectListSubject = new SelectList(subjectlst, "Id", "SubjectName")
                }
            };
            return PartialView("_DynamicAddExamMatrix", model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LoadExamMatrix(int? subjectId)
        {
            var examMatrix = await _examMatrixService.GetListAsync(subjectId);
            var selectList = new SelectList(examMatrix, "Id", "ExamMatrixName");
            return Json(selectList);
        }
    }
}
