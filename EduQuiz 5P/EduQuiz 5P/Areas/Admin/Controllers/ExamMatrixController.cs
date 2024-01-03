using EduQuiz_5P.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuiz_5P.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Roles = Constants.Roles.Admin)]
    public class ExamMatrixController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
