using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
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
    public class QuestionController : Controller
    {
        private ILogger<QuestionController> _logger;
        private readonly IQuestionService _questionService;
        private readonly IClassService _classService;
        private readonly IUserService _userService;
        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService, IUserService userService, IClassService classService)
        {
            _logger = logger;
            _questionService = questionService;
            _userService = userService;
            _classService = classService;
        }

        public async Task<IActionResult> Create()
        {
            var classlist = await _classService.GetListAsync();
            ViewData["SelectListClass"] = new SelectList(classlist, "Id", "ClassName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImportQuestionVM model)
        {
            try
            {
                await _questionService.Add(model);
                this.AddToastrMessage("Thêm dữ liệu câu hỏi thành công", ToastrMessageType.Success);
                return RedirectToAction(nameof(Create));
            }
            catch(Exception ex)
            {
                this.AddToastrMessage("Đã có lỗi xảy ra trong quá trình thêm dữ liệu câu hỏi", ToastrMessageType.Error);
                _logger.LogError(ex.Message.ToString());
            }
            var classlist = await _classService.GetListAsync();
            ViewData["SelectListClass"] = new SelectList(classlist, "Id", "ClassName");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> DynamicImportQuestion(IFormFile fileUpload, int chapterId, DifficultyLevel difficultyLevel)
        {
            ImportQuestionVM model = new ();
            try
            {
                model.ImportChapterId = chapterId;
                model.DifficultyLevel = difficultyLevel;
                if (Path.GetExtension(fileUpload.FileName) == ".tex")
                {
                    var question = await _questionService.ReadFileLatex(fileUpload);
                    model.QuestionVMs = question.ToList();
                    return PartialView("_DynamicPreview", model);
                }
                else if (Path.GetExtension(fileUpload.FileName) == ".docx")
                {
                    var question = (await _questionService.ReadFileDoc(fileUpload)) ?? new List<QuestionVM>() ;
                    model.QuestionVMs = question.ToList();
                    return PartialView("_DynamicPreview", model);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Lỗi {ex.Message}");
            }
            return NotFound();
        }
    }
}
