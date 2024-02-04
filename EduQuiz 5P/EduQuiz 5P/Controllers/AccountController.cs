using EduQuiz_5P.Data;
using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using X.PagedList;

namespace EduQuiz_5P.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserExamService _userExamService;
        public AccountController(ILogger<AccountController> logger, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IPasswordValidator<ApplicationUser> passwordValidator,
            IEmailSender emailSender, IUserStore<ApplicationUser> userStore,
            IUserEmailStore<ApplicationUser> emailStore, IUserService userService,
            IUserRegistrationService userRegistrationService, IUserExamService userExamService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _passwordValidator = passwordValidator;
            _emailSender = emailSender;
            _userStore = userStore;
            _emailStore = emailStore;
            _userService = userService;
            _userExamService = userExamService;
            _userRegistrationService = userRegistrationService;

        }

        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            UserInfoVM model = new ();
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string remoteError)
        {
            try
            {
                if (!string.IsNullOrEmpty(remoteError))
                {
                    ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                    return RedirectToAction("Login");
                }

                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    ModelState.AddModelError(string.Empty, "Error loading external login information.");
                    return RedirectToAction("Login");
                }

                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity!.Name, info.LoginProvider);
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    return RedirectToAction("Lockout");
                }
                else
                {
                    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    return RedirectToAction("ExternalLoginConfirmation", "Account", new { email = email });
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            return RedirectToAction("Login");
        }

        public IActionResult ExternalLoginConfirmation(string email)
        {
            UserInfoVM model = new()
            {
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(UserInfoVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var info = await _signInManager.GetExternalLoginInfoAsync();
                    if (info == null)
                    {
                        ModelState.AddModelError(string.Empty, "Error loading external login information during confirmation.");
                        return RedirectToAction("Login");
                    }

                    var user = new ApplicationUser { 
                        UserName = model.Email, 
                        Email = model.Email, 
                        FullName = model.FullName, 
                        Gender = model.Gender is null ? Gender.Another : model.Gender.Value, EmailConfirmed = true };
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await _userRegistrationService.Add(user.Id);
                        result = await _userManager.AddLoginAsync(user, info);
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View(model);
        }    

        [HttpPost]
        public async Task<IActionResult> Login(UserInfoVM model)
        {
            try
            {
                var returnUrl = TempData["ReturnUrl"]?.ToString() ?? Url.Content("~/");
                model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, $"Please fill in the information.");
                    return View();
                }
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred during sign-in.");
                _logger.LogError(ex.Message.ToString());
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]    
        public async Task<IActionResult> Register(UserInfoVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, $"Please fill in the information.");
                    return View();
                }
                if(model.Password != model.PasswordOld)
                {
                    ModelState.AddModelError(string.Empty, $"Password mismatch.");
                    return View();
                }    
                var user = await _userManager.FindByEmailAsync(model.Email);
                var username = await _userManager.FindByNameAsync(model.UserName);
                if (username != null || user != null)
                {
                    ModelState.AddModelError(string.Empty, $"{(username != null ? "Your user name" : "Your email") } just entered already exists.");
                    return View();
                }
                user = new ApplicationUser()
                {
                    UserName= model.UserName,
                };
                var result = await _passwordValidator.ValidateAsync(_userManager, user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                Random random = new();
                var code = random.Next(1000, 9999);
                VerifyEmailVM verifyEmail = new ()
                {
                    User = model,
                    Code = code,
                    ExpiryCode = DateTime.UtcNow.AddMinutes(5).ToTimeZone(),
                    EmailType = VerifyEmailType.Register,
                    Email = model.Email ?? ""
                };
                await _emailSender.SendEmailAsync(model.Email, "[LuyenToan.Online] Xác nhận email", CallBack.GetMailHtml(model.Email, model.FullName, code));
                HttpContext.Session.SetObjectAsJson(Constants.CodeSession, verifyEmail);
                return RedirectToAction("EmailConfirmation", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred during registration.");
                _logger.LogInformation(ex.Message.ToString());
            }
            return View(model);
        }

        public IActionResult EmailConfirmation()
        {
            var model = HttpContext.Session.GetObjectFromJson<VerifyEmailVM>(Constants.CodeSession) ?? new VerifyEmailVM();
            if (model == null)
            {
                return View();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailConfirmation(int code)
        {
            try
            {
                var model = HttpContext.Session.GetObjectFromJson<VerifyEmailVM>(Constants.CodeSession) ?? new VerifyEmailVM();
                if (model == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if(model.ExpiryCode <= DateTime.UtcNow.ToTimeZone() || model.Code != code)
                {
                    ModelState.AddModelError(string.Empty, "Mã code không tồn tại.");
                    return View();
                }    
                switch(model.EmailType)
                {
                    case VerifyEmailType.Register:
                    {
                        if(model.User == null)
                        {
                            this.AddToastrMessage("Đã có lỗi xảy ra, vui lòng đăng ký lại.", ToastrMessageType.Error);
                            return RedirectToAction(nameof(Register));
                        }    
                        var user = CreateUser();
                        user.FullName = model.User.FullName ?? string.Empty;
                        user.Gender = model.User.Gender ?? Gender.Another;
                        await _userStore.SetUserNameAsync(user, model.User.UserName, CancellationToken.None);
                        await _emailStore.SetEmailAsync(user, model.User.Email, CancellationToken.None);
                        user.EmailConfirmed = true;
                        var result = await _userManager.CreateAsync(user, model.User.Password);
                        if (result.Succeeded)
                        {
                            await _userRegistrationService.Add(user.Id);
                            HttpContext.Session.Remove(Constants.CodeSession);
                            return RedirectToAction("Login", "Account");
                        }
                        ModelState.AddModelError(string.Empty, "Confirm Email Not Success.");
                        break;
                    }
                    case VerifyEmailType.Password:
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View();

        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            try
            {
                var user = await _userService.GetUser();
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập.");
                    return View();
                }
                return View(new UserInfoVM(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserInfoVM model)
        {
            try
            {
                if (!model.UserId.HasValue)
                {
                    this.AddToastrMessage("Bạn cần đăng nhập và thử lại.", ToastrMessageType.Error);
                    return RedirectToAction(nameof(Profile));
                }
                var user = await _userService.GetUser(model.UserId.Value);
                if (user is null)
                {
                    this.AddToastrMessage("Bạn cần đăng nhập và thử lại.", ToastrMessageType.Error);
                    return RedirectToAction(nameof(Profile));
                }
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.PasswordOld, model.Password);
                if (!changePasswordResult.Succeeded)
                {
                    this.AddToastrMessage("Thay đổi mật khẩu thành công", ToastrMessageType.Success);
                    return RedirectToAction(nameof(Profile));
                }
                ModelState.AddModelError(string.Empty, "Thay đổi mật khẩu không thành công, có vẻ bạn nhập sai mật khẩu.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var user = await _userService.GetUser();
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập.");
                    return View();
                }
                return View(new UserInfoVM(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(long? userId, UserInfoVM model)
        {
            try
            {
                await _userService.UpdateUser(model);
                this.AddToastrMessage("Cập nhật thông tin thành công.", ToastrMessageType.Success);
                return View(nameof(Profile));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LogOut(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> HistoryExam(int? page)
        {
            try
            {
                var user = await _userService.GetUser();
                if(user ==  null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var userExam = await _userExamService.GetListAsync(user.Id);
                int pagesize = 4;
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                var model = new PagedList<UserExamInfoVM>(userExam.Select(x => new UserExamInfoVM(x)).ToList(), pagenumber, pagesize);
                return View(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View();
        }    

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
