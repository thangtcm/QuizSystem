using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduQuiz_5P.Areas.Admin.Controllers
{
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
            ICollection<Classes> model = new List<Classes>();
            model.Add(new Classes());

            return View(model.ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ICollection<Classes> classes)
        {
            try
            {
                var user = await _userService.GetUser();
                if(user == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Dữ liệu rỗng.");
                    return View();
                }
                await _classService.AddRange(classes, user.Id);
                this.AddToastrMessage("Tạo lớp thành công", ToastrMessageType.Success);
                return RedirectToAction(nameof(Index));
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                this.AddToastrMessage("Đã có lỗi xảy ra", ToastrMessageType.Error);

            }
            return View(classes);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var @class = await _classService.GetByIdAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            return View(@class);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Classes @class)
        {
            try
            {
                await _classService.Update(@class);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Khi cập nhật Một Class : \n" + ex.ToString() + "\n\n");
            }
            return View(@class);
        }

        public IActionResult CreatePartial()
        {
            ICollection<Classes> model = new List<Classes>();
            model.Add(new Classes());
            return PartialView("_DynamicAddClass", model.ToList());
        }
    }
}
