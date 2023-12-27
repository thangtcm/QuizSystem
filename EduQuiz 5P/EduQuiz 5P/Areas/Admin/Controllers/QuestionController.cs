using EduQuiz_5P.Services.Interface;
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

        public IActionResult Index()
        {
            return View();
        }
    }
}
