using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduQuiz_5P.Data;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using EduQuiz_5P.Enums;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Authorize]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleService _roleService;
        public AccountController(IUserService userService, ILogger<AccountController> logger, IRoleService roleService,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _logger = logger;
            _roleService = roleService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userService.GetUsersWithRoles();
                this.AddToastrMessage("Tải dữ liệu thành công", ToastrMessageType.Success);   
                return View(users);
            }catch(Exception ex)
            {
                this.AddToastrMessage("Đã có lỗi xayr ra", ToastrMessageType.Error);
                _logger.LogError(ex.Message.ToString());
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles =Constants.Roles.Admin)]
        public async Task<IActionResult> Edit(long userId)
        {
            var user = await _userService.GetUser(userId);
            var roles = await _roleService.GetRoles();

            if (user == null)
                return NotFound();

            var vm = new EditUserViewModel
            {
                User = user,
                Roles = new List<SelectListItem>()
            };
            foreach (var role in roles)
            {
                SelectListItem item = new()
                {
                    Text = role.Name,
                    Value = role.Id.ToString(),
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
                vm.Roles.Add(item);
            }    
            return View(vm);
        }

        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel data)
        {
            var user = await _userService.GetUser(data.User!.Id);
            try
            {
                if (user == null)
                {
                    return NotFound();
                }

                var roles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.RemoveFromRolesAsync(user, roles);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user existing roles");
                    return View(data);
                }
                result = await _userManager.AddToRolesAsync(user, data.Roles!.Where(x => x.Selected).Select(y => y.Text));
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected roles to user");
                    return View(data);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("Edit", new { id = user!.Id });
        }
    }
}
