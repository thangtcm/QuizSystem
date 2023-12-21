using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuiz_5P.Areas.Admin.Controllers
{
    [Authorize]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    [Area("Admin")]
    public class ClassController : Controller
    {
        private ILogger<ClassController> _logger;
        private readonly IClassService _classService;
        private readonly IUserService _userService;
        public ClassController(ILogger<ClassController> logger, IClassService classService, IUserService userService)
        {
            _logger = logger;
            _classService = classService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _classService.GetListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ICollection<Classes> model)
        {
            try
            {
                var user = await _userService.GetUser();
                if(user == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                await _classService.AddRange(model, user.Id);
                this.AddToastrMessage("Tạo lớp thành công", ToastrMessageType.Success);
                return RedirectToAction(nameof(Index));
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                this.AddToastrMessage("Đã có lỗi xảy ra", ToastrMessageType.Error);

            }
            return View(model);
        }
    }
}
