using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;

namespace EduQuiz_5P.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Roles = Constants.Roles.Admin)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IQuestionService _questionService;
        private readonly IExamService _examService;
        private readonly IUserExamService _userExamService;
        private readonly IUserRegistrationService _userRegistrationService;
        public HomeController(ILogger<HomeController> logger, IUserService userService, IQuestionService questionService, IExamService examService, IUserExamService userExamService, IUserRegistrationService userRegistrationService)
        {
            _logger = logger;
            _userService = userService;
            _questionService = questionService;
            _examService = examService;
            _userExamService = userExamService;
            _userRegistrationService = userRegistrationService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.CountAsync();
            var questions = await _questionService.CountAsync();
            var exams = await _examService.CountAsync();
            var userexams = await _userExamService.CountAsync();
            var monthUserRegister = await _userRegistrationService.GetListAysnc();
            var userTopRank = await _userService.GetTopRank();
            StatisticalVM model = new()
            {
                NumberOfUser = users,
                NumberOfQuestion = questions,
                NumberOfExam = exams,
                NumberOfUserExam = userexams,
                UserRegisterData = monthUserRegister,
                UserTopRank = userTopRank
            };
            return View(model);
        }
    }
}
