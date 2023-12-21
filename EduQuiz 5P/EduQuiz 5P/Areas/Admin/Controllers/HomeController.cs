using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduQuiz_5P.Helpers;

namespace EduQuiz_5P.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Roles = Constants.Roles.Admin)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
