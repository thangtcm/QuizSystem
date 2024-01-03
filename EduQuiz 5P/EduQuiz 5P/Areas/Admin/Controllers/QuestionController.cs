using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
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
            TempData["SelectListClass"] = new SelectList(classlist, "Id", "ClassName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ImportQuestionVM model)
        {
            try
            {
                ICollection<QuestionVM> Questionlst = new List<QuestionVM>();
                if(Path.GetExtension(model.UploadFile.FileName) == ".tex")
                {
                    Questionlst = await _questionService.ReadFileLatex(model.UploadFile);
                }
                else if(Path.GetExtension(model.UploadFile.FileName) == ".docs")
                {

                }
                model.QuestionVMs = Questionlst;
                await _questionService.Add(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DynamicImportQuestion(IFormFile fileUpload, int chapterId, DifficultyLevel difficultyLevel)
        {
            ImportQuestionVM model = new ImportQuestionVM();
            try
            {
                model.ChapterId = chapterId;
                model.DifficultyLevel = difficultyLevel;
                if (Path.GetExtension(fileUpload.FileName) == ".tex")
                {
                    var question = await _questionService.ReadFileLatex(fileUpload);
                    model.QuestionVMs = question;
                }
                else if (Path.GetExtension(model.UploadFile.FileName) == ".docx")
                {
                    var question = _questionService.ReadFileDoc(fileUpload);
                    model.QuestionVMs = question;
                }
            }
            catch(Exception)
            {

            }
            return PartialView("_DynamicPreview", model);
        }
    }
}
