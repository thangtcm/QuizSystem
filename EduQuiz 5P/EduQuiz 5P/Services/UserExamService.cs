using DocumentFormat.OpenXml.Office2010.Excel;
using EduQuiz_5P.Helpers;
using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using EduQuiz_5P.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EduQuiz_5P.Services
{
    public class UserExamService : IUserExamService
    {
        public IUnitOfWork _unitOfWork { get; set; }
        private readonly IExamService _examService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        public UserExamService(IUnitOfWork unitOfWork, IExamService examService, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _examService = examService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }
        public async Task<UserExamInfoVM?> Add(int examId, long? userId)
        {
            var exam = await _examService.GetByIdAsync(examId, x => x.Include(q => q.Questions!).ThenInclude(a => a.Answers!));
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
                var userExamDetail = exam.Questions.Select(x => new UserExamDetail() { Question = x, UserExam = userExams }).ToList();
                _unitOfWork.UserExamRepository.Add(userExams);
                await _unitOfWork.UserExamDetailRepository.AddRangeAsync(userExamDetail);
                await _unitOfWork.CommitAsync();
                model.UserExamDetailVM = userExamDetail.Select(x => new UserExamDetailInfoVM(x)).ToList();
                model.IsSession = false;
                model.UserExamId = userExams.Id;
            }
            else
            {
                var userExamDetail = exam.Questions.Select(x => new UserExamDetail() { Question = x, UserExamId = 0}).ToList();
                model.UserExamDetailVM = userExamDetail.Select(x => new UserExamDetailInfoVM(x)).ToList();
                _httpContextAccessor.HttpContext!.Session.SetObjectAsJson(Constants.ExamSession, model);
            }
            return model;
        }

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
                        .Where(detail => detail.Answer != null && detail.Answer.IsCorrect)
                        .Count();
                    model.EndTime = DateTime.UtcNow.ToTimeZone();
                    _unitOfWork.UserExamRepository.Update(model);
                    await _unitOfWork.CommitAsync();
                    return new UserExamInfoVM(model);
                }
            }
            return null;
        }

        public Task<ICollection<UserExams>> GetListAsync(long? userId, Func<IQueryable<UserExams>, IIncludableQueryable<UserExams, object>>? includes = null, DateTime? date = null, bool IsOrder = false)
        {
            throw new NotImplementedException();
        }
    }
}
