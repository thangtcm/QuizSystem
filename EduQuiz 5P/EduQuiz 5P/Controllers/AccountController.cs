using EduQuiz_5P.Data;
using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.ViewModel;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

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
        public AccountController(ILogger<AccountController> logger, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IPasswordValidator<ApplicationUser> passwordValidator,
            IEmailSender emailSender, IUserStore<ApplicationUser> userStore,
            IUserEmailStore<ApplicationUser> emailStore)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _passwordValidator = passwordValidator;
            _emailSender = emailSender;
            _userStore = userStore;
            _emailStore = emailStore;
        }

        public async Task<IActionResult> Login()
        {
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
                    return RedirectToAction("Index", "Home");
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
                UserCode userCode = new ()
                {
                    User = model,
                    Code = code,
                    DateSend = DateTime.UtcNow.AddMinutes(5).ToTimeZone()
                };
                await _emailSender.SendEmailAsync(model.Email, "[LuyenToan.Online] Xác nhận email", CallBack.GetMailHtml(model.Email, model.FullName, code));
                var data = JsonConvert.SerializeObject(userCode);
                Console.WriteLine($"Data là {data}");
                HttpContext.Session.SetString(Constants.CodeSession, data);
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
            var model = HttpContext.Session.GetString(Constants.CodeSession);
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
                var model = HttpContext.Session.GetString(Constants.CodeSession);
                if (model == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var userCode = JsonConvert.DeserializeObject<UserCode>(model);
                if(userCode == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if(userCode.DateSend <= DateTime.UtcNow.ToTimeZone() || userCode.Code != code)
                {
                    ModelState.AddModelError(string.Empty, "Authentication code does not exist.");
                    return View();
                }    
                else
                {
                    var user = CreateUser();
                    user.FullName = userCode.User.FullName ?? string.Empty;
                    user.Gender = userCode.User.Gender ?? Gender.Another;
                    await _userStore.SetUserNameAsync(user, userCode.User.UserName, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, userCode.User.Email, CancellationToken.None);
                    user.EmailConfirmed = true;
                    var result = await _userManager.CreateAsync(user, userCode.User.Password);
                    if (result.Succeeded)
                    {
                        HttpContext.Session.Remove(Constants.CodeSession);
                        return RedirectToAction("Login", "Account");
                    }
                    ModelState.AddModelError(string.Empty, "Confirm Email Not Success.");
                    return View();
                }

            }
            catch(Exception ex)
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
