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
    public class ExamService : IExamService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IFirebaseStorageService _firebaseStorageService;
        public ExamService(IUnitOfWork unitOfWork, IUserService userService, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<ICollection<Exam>> GetExamDefaultList(int? ClassId = null, int? SubjectId = null, int? ChapterId = null, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null)
        {
            if (ClassId.HasValue && SubjectId.HasValue && ChapterId.HasValue)
            {
                return await _unitOfWork.ExamRepository.GetAllAsync(e => e.IsDefault && e.ChapterId == ChapterId, includes);
            }
            else if (ClassId.HasValue && SubjectId.HasValue && !ChapterId.HasValue)
            {
                return await _unitOfWork.ExamRepository.GetAllAsync(e => e.IsDefault && e.SubjectId == SubjectId, includes);
            }
            else if (ClassId.HasValue && !SubjectId.HasValue && !ChapterId.HasValue)
            {
                return await _unitOfWork.ExamRepository.GetAllAsync(e => e.IsDefault && e.ClassId == ClassId, includes);
            }
            else
                return await _unitOfWork.ExamRepository.GetAllAsync(e => e.IsDefault);
        }

        public async Task<ICollection<Exam>> GetExamOwnerList(long userId, int? ClassId = null, int? SubjectId = null, int? ChapterId = null, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null)
        {
            if (ClassId.HasValue && SubjectId.HasValue && ChapterId.HasValue)
            {
                return await _unitOfWork.ExamRepository.GetAllAsync(e => e.IsDefault == false && e.ChapterId == ChapterId && e.UserIdCreate == userId, includes);
            }
            else if (ClassId.HasValue && SubjectId.HasValue && !ChapterId.HasValue)
            {
                return await _unitOfWork.ExamRepository.GetAllAsync(e => e.IsDefault == false && e.SubjectId == SubjectId, includes);
            }
            else if (ClassId.HasValue && !SubjectId.HasValue && !ChapterId.HasValue)
            {
                return await _unitOfWork.ExamRepository.GetAllAsync(e => e.IsDefault == false && e.ClassId == ClassId, includes);
            }
            else
                return await _unitOfWork.ExamRepository.GetAllAsync(e => e.IsDefault == false);
        }

        public Task CreateExamWithMatrix(Exam exam, int examMatrixId)
        {
            throw new NotImplementedException();
        }

        public Task<Exam?> CreateExamMatrixDefault(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<Exam> CreateExam(int NumberOfQuestions, int ExamTime, int? ClassId = null, int? SubjectId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Exam>> GetListAsync(Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null)
            => await _unitOfWork.ExamRepository.GetAllAsync(null, includes);

        public async Task<Exam?> GetByIdAsync(int Id)
           => await _unitOfWork.ExamRepository.GetAsync(x => x.Id == Id);

        public async Task<Exam?> GetByIdAsync(int Id, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null)
            => await _unitOfWork.ExamRepository.GetAsync(x => x.Id == Id, includes);
        public Exam? GetById(int id)
            => _unitOfWork.ExamRepository.Get(x => x.Id == id);
        public async Task DeleteExam(int examId)
        {
            var exam = await _unitOfWork.ExamRepository.GetAsync(x => x.Id == examId);
            if(exam != null)
            {
                _unitOfWork.ExamRepository.Remove(exam);
                await _unitOfWork.CommitAsync();
            }    
        }

        public async Task CreateExamImport(ImportExamFileVM model)
        {
            ICollection<Question> questions = new List<Question>();
            ICollection<Answer> answers = new List<Answer>();
            var user = await _userService.GetUser();
            foreach (var questionvm in model.QuestionVMs)
            {
                var urlImageQuestion = questionvm.UrlImage;
                var urlImageSolution = questionvm.UrlImageSolution;
                if (questionvm.UploadImageQuestion != null)
                {
                    urlImageQuestion = (await _firebaseStorageService.UploadFile(questionvm.UploadImageQuestion)).ToString();
                }
                if (questionvm.UploadImageQuestionSolution != null)
                {
                    urlImageSolution = (await _firebaseStorageService.UploadFile(questionvm.UploadImageQuestionSolution)).ToString();
                }
                Question question = new()
                {
                    QuestionName = questionvm.QuestionName,
                    QuestionHints = questionvm.QuestionHints,
                    QuestionSolution = questionvm.QuestionSolution,
                    IsImage = urlImageQuestion,
                    IsImageSolution = urlImageSolution,
                    DateUpdate = DateTime.UtcNow.ToTimeZone(),
                    UserUpdate = user
                };
                questions.Add(question);
                foreach (var ansVm in questionvm.AnswerList)
                {
                    Answer answer = new()
                    {
                        Question = question,
                        AnswerName = ansVm.AnswerName,
                        IsCorrect = ansVm.IsCorrect,
                        DateUpdate = DateTime.UtcNow.ToTimeZone(),
                        UserUpdate = user
                    };
                    answers.Add(answer);
                }
            }
            _unitOfWork.QuestionRepository.AddRange(questions);
            _unitOfWork.AnswerRepository.AddRange(answers);
            await _unitOfWork.CommitAsync();
            var exam = new Exam()
            {
                ExamType = model.ExamType,
                ExamDescription = model.ExamDescription,
                ExamName = model.ExamName,
                ExamTime = model.ExamTime,
                NumberOfQuestion = questions.Count,
                IsRemoved = false,
                DateCreate = DateTime.UtcNow.ToTimeZone(),
                ListQuestion = string.Join(", ", questions.Select(x => x.Id).ToList()),
                TotalUserExam = 0,
                SubjectId = model.ExamSubjectId
            };
            if(model.ExamType == Enums.ExamType.Lop)
            {
                exam.ClassId = model.ExamClassId;
            }
            _unitOfWork.ExamRepository.Add(exam);
            await _unitOfWork.CommitAsync();
        }
    }
}
