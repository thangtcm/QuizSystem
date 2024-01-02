using EduQuiz_5P.Enums;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EduQuiz_5P.Areas.Admin.Controllers
{
    public class QuestionController : Controller
    {
        private ILogger<QuestionController> _logger;
        private IQuestionService _questionService;
        private readonly IUserService _userService;
        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService, IUserService userService)
        {
            _logger = logger;
            _questionService = questionService;
            _userService = userService;
        }

        public IActionResult Create()
        {
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
    }
}
