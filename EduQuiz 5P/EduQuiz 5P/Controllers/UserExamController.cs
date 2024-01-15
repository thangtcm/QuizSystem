using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Services;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace EduQuiz_5P.Controllers
{
    public class UserExamController : Controller
    {
        private readonly IUserExamService _userExamService;
        private readonly ILogger<UserExamController> _logger;
        private readonly IUserService _userService;
        private readonly IExamService _examService;
        private readonly IUserExamDetailService _userExamDetailService;
        public UserExamController(IUserExamService userExamService, ILogger<UserExamController> logger, IUserService userService, IExamService examService, IUserExamDetailService userExamDetailService)
        {
            _userExamService = userExamService;
            _logger = logger;
            _userService = userService;
            _examService = examService;
            _userExamDetailService = userExamDetailService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateExamMatrix(UserExamGenerate model, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            try
            {
                var user = await _userService.GetUser();
                if(user != null)
                {
                    var result = await _userExamService.GenerateExamMatrix(model, user.Id);
                    if (!result.IsSuccess)
                    {
                        this.AddToastrMessage(result.Message, Enums.ToastrMessageType.Error);
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction(nameof(TakeExam), new { userExamId = result.Result!.UserExamId, returnUrl = returnUrl });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> Take(int examId, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            UserExamInfoVM model = new();
            try
            {
                var user = await _userService.GetUser();
                var result = await _userExamService.Add(examId, user?.Id);
                if (result == null)
                {
                    this.AddToastrMessage("Đã có lỗi xảy ra, vui lòng thao tác lại.", Enums.ToastrMessageType.Error);
                    return LocalRedirect(returnUrl);
                }
                model = result;
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction(nameof(TakeExam), new { userExamId = model.UserExamId, returnUrl = returnUrl });
        }

        public async Task<IActionResult> TakeExam(int? userExamId, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            try
            {
                UserExamInfoVM model = new();
                if (userExamId.HasValue && userExamId.Value > 0)
                {
                    var user = await _userService.GetUser();
                    if(user != null)
                    {
                        var userExams = await _userExamService.GetByIdAsync(userExamId.Value, user.Id, x => x.Include(d => d.UserExamDetails!).ThenInclude(q => q.Question!).ThenInclude(a => a.Answers!));
                        if (userExams != null)
                        {
                            model = new UserExamInfoVM(userExams)
                            {
                                QuestionComplete = userExams.UserExamDetails!.Count(x => x.SelectAnswerId.HasValue)
                            };
                            this.AddToastrMessage("Hệ thống đã bắt đầu bài thi cho bạn.", Enums.ToastrMessageType.Success);
                            return View(model);
                        }
                    }
                }
                else
                {
                    model = HttpContext!.Session.GetObjectFromJson<UserExamInfoVM>(Constants.ExamSession) ?? new UserExamInfoVM();
                    if(model != null)
                    {
                        this.AddToastrMessage("Hệ thống đã bắt đầu bài thi cho bạn.", Enums.ToastrMessageType.Success);
                        return View(model);
                    }    
                }
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            this.AddToastrMessage("Đã có lỗi xảy ra, vui lòng thao tác lại.", Enums.ToastrMessageType.Error);
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> SelectAnswer(int UserExamDetailId, int QuestionId, int AnswerId)
        {
            try
            {
                if(UserExamDetailId != 0)
                {
                    await _userExamDetailService.SelectAnswer(UserExamDetailId, AnswerId);
                }
                else
                {
                    var model = HttpContext!.Session.GetObjectFromJson<UserExamInfoVM>(Constants.ExamSession) ?? new UserExamInfoVM();
                    if(model != null)
                    {
                        var userExamDetails = model.UserExamDetailVM.FirstOrDefault(x => x.QuestionVM.QuestionId == QuestionId);
                        if(userExamDetails != null)
                        {
                            userExamDetails.SelectAnswerId = AnswerId;
                            HttpContext!.Session.SetObjectAsJson(Constants.ExamSession, model);
                        }
                    }
                }
                return Ok();
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return NotFound();
        }

        public async Task<IActionResult> GetQuestionComplete(int UserExamId)
        {
            int questionComplete = 0;
            try
            {
                if (UserExamId != 0)
                {
                    questionComplete = await _userExamService.CountQuestionComplete(UserExamId);
                }
                else
                {
                    var model = HttpContext!.Session.GetObjectFromJson<UserExamInfoVM>(Constants.ExamSession) ?? new UserExamInfoVM();
                    if (model != null)
                    {
                        questionComplete = model.UserExamDetailVM.Count(x => x.SelectAnswerId.HasValue);
                        model.QuestionComplete = questionComplete;
                      
                        HttpContext!.Session.SetObjectAsJson(Constants.ExamSession, model);
                    }
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Json(questionComplete);
        }

        [HttpPost]
        public async Task<IActionResult> Result(int UserExamId, string? returnUrl)
        {
            try
            {
                returnUrl ??= Url.Content("~/");
                var result = await _userExamService.Submit(UserExamId);
                if(result == null)
                {
                    return LocalRedirect(returnUrl);
                }    
                return View(result);
            }catch(Exception ex) {
                _logger.LogError(ex.Message);
            }
            return View();
        }
    }
}
