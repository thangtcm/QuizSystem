using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EduQuiz_5P.Services
{
    public class UserExamService : IUserExamService
    {
        public IUnitOfWork _unitOfWork { get; set; }
        private readonly IExamService _examService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IQuestionService _questionService;
        public UserExamService(IUnitOfWork unitOfWork, IExamService examService, IHttpContextAccessor httpContextAccessor, IUserService userService, IQuestionService questionService)
        {
            _unitOfWork = unitOfWork;
            _examService = examService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _questionService = questionService;
        }

        public async Task<ResponResultData<UserExamInfoVM>> GenerateExamMatrix(UserExamGenerate model, long userId)
        {
            ResponResultData<UserExamInfoVM> result = new();
            if (model.ExamMatrixId.HasValue)
            {
                var examMatrix = await _unitOfWork.ExamMatrixRepository.GetAsync(x => x.Id == model.ExamMatrixId.Value, x => x.Include(ed => ed.ExamMatrixDetail!));
                if (examMatrix != null && examMatrix.ExamMatrixDetail != null)
                {
                    var questions = await _questionService.GenerateQuestion(examMatrix.Id);
                    if (!questions.IsSuccess || questions.ListResult == null)
                    {
                        result.IsSuccess = false;
                        result.Message = questions.Message;
                        return result;
                    }
                    UserExams userExams = new ();
                    ICollection<UserExamDetail> userExamDetails = new List<UserExamDetail>();
                    foreach (var q in questions.ListResult)
                    {
                        UserExamDetail examDetail = new()
                        {
                            UserExam = userExams,
                            QuestionId = q.Id
                        };
                        userExamDetails.Add(examDetail);
                    }
                    userExams.ExamName = $"Thi ngẫu nhiên {DateTime.UtcNow.ToTimeZone().ToString("dd/MM/yyyy HH:mm")}";
                    userExams.NumberOfQuestion = questions.ListResult.Count;
                    userExams.Identification = questions.ListResult.Count(x => x.DifficultyLevel == DifficultyLevel.RECOGNITION);
                    userExams.Understanding = questions.ListResult.Count(x => x.DifficultyLevel == DifficultyLevel.UNDERSTANDING);
                    userExams.Application = questions.ListResult.Count(x => x.DifficultyLevel == DifficultyLevel.APPLICATION);
                    userExams.AdvancedApplication = questions.ListResult.Count(x => x.DifficultyLevel == DifficultyLevel.HIGHER_ORDER_APPLICATION);
                    userExams.StartTime = DateTime.UtcNow.ToTimeZone();
                    userExams.EndTime = DateTime.UtcNow.ToTimeZone().AddMinutes(model.ExamTime + 5);
                    await _unitOfWork.UserExamRepository.AddAsync(userExams);
                    await _unitOfWork.UserExamDetailRepository.AddRangeAsync(userExamDetails);
                    await _unitOfWork.CommitAsync();
                    userExams.UserExamDetails = userExamDetails;
                    result.IsSuccess = true;
                    result.Message = "Sinh đề thành công.";
                    result.Result = new UserExamInfoVM(userExams);
                    return result;
                }
            }
            else
            {
                ICollection<Question> randomQuestions;
                randomQuestions = await _unitOfWork.QuestionRepository.GetAllAsync(
                    x => x.IsRemoved == false && x.Chappter!.ClassesId == model.ClassId,
                    x => x.Include(c => c.Chappter!),
                    query => query.OrderBy(q => Guid.NewGuid()),
                    model.NumberOfQuestion
                );
                UserExams userExams = new();
                ICollection<UserExamDetail> userExamDetails = new List<UserExamDetail>();
                foreach (var q in randomQuestions)
                {
                    UserExamDetail examDetail = new()
                    {
                        UserExam = userExams,
                        QuestionId = q.Id
                    };
                    userExamDetails.Add(examDetail);
                }
                userExams.NumberOfQuestion = randomQuestions.Count;
                userExams.Identification = randomQuestions.Count(x => x.DifficultyLevel == DifficultyLevel.RECOGNITION);
                userExams.Understanding = randomQuestions.Count(x => x.DifficultyLevel == DifficultyLevel.UNDERSTANDING);
                userExams.Application = randomQuestions.Count(x => x.DifficultyLevel == DifficultyLevel.APPLICATION);
                userExams.AdvancedApplication = randomQuestions.Count(x => x.DifficultyLevel == DifficultyLevel.HIGHER_ORDER_APPLICATION);
                userExams.StartTime = DateTime.UtcNow.ToTimeZone();
                userExams.EndTime = DateTime.UtcNow.ToTimeZone().AddMinutes(model.ExamTime + 5);
                await _unitOfWork.UserExamRepository.AddAsync(userExams);
                await _unitOfWork.UserExamDetailRepository.AddRangeAsync(userExamDetails);
                await _unitOfWork.CommitAsync();
                userExams.UserExamDetails = userExamDetails;
                result.IsSuccess = true;
                result.Message = "Sinh đề thành công.";
                result.Result = new UserExamInfoVM(userExams);
                return result;
            }
            result.IsSuccess = false;
            result.Message = "Tài nguyên không đủ để sinh đề.";
            return result;
        }

        public async Task<UserExamInfoVM?> Add(int examId, long? userId)
        {
            var exam = await _examService.GetByIdAsync(examId, x => x.Include(ed => ed.ExamDetail!).ThenInclude(q => q.Question!).ThenInclude(a => a.Answers!));
            if (exam == null)
            {
                return null;
            }
            UserExamInfoVM model = new() { 
                ExamName = exam.ExamName ?? "",
                UserId = userId,
                StartTime = DateTime.UtcNow.ToTimeZone(),
                EndTime = DateTime.UtcNow.ToTimeZone().AddMinutes(exam.ExamTime + 5),
                NumberOfQuestion = exam.NumberOfQuestion
            };
            
            if(userId.HasValue)
            {
                UserExams userExams = new()
                {
                    UserId = userId.Value
                };
                userExams.CreateUserExam(exam);
                var userExamDetail = exam.ExamDetail.Select(x => new UserExamDetail() { Question = x.Question, UserExam = userExams }).ToList();
                _unitOfWork.UserExamRepository.Add(userExams);
                await _unitOfWork.UserExamDetailRepository.AddRangeAsync(userExamDetail);
                await _unitOfWork.CommitAsync();
                model.UserExamDetailVM = userExamDetail.Select(x => new UserExamDetailInfoVM(x)).ToList();
                model.IsSession = false;
                model.UserExamId = userExams.Id;
            }
            else
            {
                var userExamDetail = exam.ExamDetail.Select(x => new UserExamDetail() { Question = x.Question, UserExamId = 0}).ToList();
                model.UserExamDetailVM = userExamDetail.Select(x => new UserExamDetailInfoVM(x)).ToList();
                _httpContextAccessor.HttpContext!.Session.SetObjectAsJson(Constants.ExamSession, model);
            }
            return model;
        }

        public async Task<int> CountAsync()
            => await _unitOfWork.UserExamRepository.CountAsync();
        public async Task<int> CountCorrectAnswers(int Id)
        {
            var userExam = await _unitOfWork.UserExamRepository.GetAsync(x => x.Id == Id, x => x.Include(d => d.UserExamDetails!).ThenInclude(a => a.Answer));

            if (userExam != null)
            {
                int count = userExam.UserExamDetails!
                    .Where(detail => detail.Answer != null && detail.Answer.IsCorrect)
                    .Count();

                return count;
            }

            return 0;
        }

        public async Task<int> CountQuestionComplete(int Id)
        {
            int count = await _unitOfWork.UserExamDetailRepository.CountAsync(x => x.UserExamId == Id && x.SelectAnswerId != null);
            return count;
        }

        public async Task<bool> Delete(int Id)
        {
            var userExam = await _unitOfWork.UserExamRepository.GetAsync(x => x.Id == Id);
            if (userExam == null) return false;
            var userExamDetails = await _unitOfWork.UserExamDetailRepository.GetAllAsync(x => x.UserExamId == userExam.Id);
            try
            {
                if (userExamDetails != null) _unitOfWork.UserExamDetailRepository.RemoveRange(userExamDetails);
                _unitOfWork.UserExamRepository.Remove(userExam);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Remove UserExam : " + ex.ToString());
                return false;
            }
        }

        public UserExams? GetById(int Id)
            => _unitOfWork.UserExamRepository.Get(x => x.Id == Id);

        public async Task<UserExams?> GetByIdAsync(int Id, long? userId, Func<IQueryable<UserExams>, IIncludableQueryable<UserExams, object>>? includes = null)
            => await _unitOfWork.UserExamRepository.GetAsync(x => x.Id == Id && x.UserId == (userId ?? 0), includes);

        public async Task<UserExamInfoVM?> Submit(int Id)
        {
            if (Id == 0)
            {
                var model = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<UserExamInfoVM>(Constants.ExamSession) ?? new UserExamInfoVM();
                if(model != null)
                {
                    model.NumberOfCorrect = model.UserExamDetailVM.Count(x => x.QuestionVM.AnswerList.Any(a => a.AnswerId == x.SelectAnswerId && a.IsCorrect == true));
                    return model;
                }
                return null;
            }
            else
            {
                var model = await _unitOfWork.UserExamRepository.GetAsync(x => x.Id == Id, x => x.Include(d => d.UserExamDetails!).ThenInclude(x => x.Question!).ThenInclude(a => a.Answers!));
                if (model != null)
                {
                    model.NumberOfCorrect = model.UserExamDetails!
                        .Where(detail => detail.Answer != null && detail.Answer.IsCorrect == true)
                        .Count();
                    model.EndTime = DateTime.UtcNow.ToTimeZone();
                    var user = await _userService.GetUser(model.UserId);
                    if(user != null)
                    {
                        user.Point += model.NumberOfCorrect;
                        _unitOfWork.UserRepository.Update(user);
                    }
                    _unitOfWork.UserExamRepository.Update(model);
                    await _unitOfWork.CommitAsync();
                    return new UserExamInfoVM(model);
                }
            }
            return null;
        }

        public async Task<ICollection<UserExams>> GetListAsync(long? userId = null, Func<IQueryable<UserExams>, IIncludableQueryable<UserExams, object>>? includes = null, DateTime? date = null, bool IsOrder = false)
        {
            var model = await _unitOfWork.UserExamRepository.GetAllAsync(
                x => (!userId.HasValue || x.UserId == userId.Value) && (!date.HasValue || x.StartTime.Day == date.Value.Day),
                includes,
                IsOrder ? x => x.OrderByDescending(x => x.Id) : null
            );
            return model;
        }
    }
}
