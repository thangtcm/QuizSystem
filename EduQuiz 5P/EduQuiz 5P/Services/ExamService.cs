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
    public class ExamService : IExamService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IExamMatrixService _examMatrixService;
        private readonly IQuestionService _questionService;
        public ExamService(IUnitOfWork unitOfWork, IUserService userService, IFirebaseStorageService firebaseStorageService, IExamMatrixService examMatrixService, IQuestionService questionService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _firebaseStorageService = firebaseStorageService;
            _examMatrixService = examMatrixService;
            _questionService = questionService;
        }

        public async Task<ICollection<Exam>> GetExamDefaultList(int? ClassId = null, int? SubjectId = null, int? ChapterId = null, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null)
        {
            var exams = await _unitOfWork.ExamRepository.GetAllAsync(e => e.IsDefault, includes);

            var query = exams.AsQueryable();

            if (ClassId.HasValue)
            {
                query = query.Where(e => e.ClassId == ClassId);
            }

            if (SubjectId.HasValue)
            {
                query = query.Where(e => e.SubjectId == SubjectId);
            }

            if (ChapterId.HasValue)
            {
                query = query.Where(e => e.ChapterId == ChapterId);
            }
            return query.ToList();
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

        public async Task<ResponResultData<Exam>> CreateExamWithMatrix(Exam exam, int examMatrixId, long userId)
        {
            ResponResultData<Exam> result = new();

            var examMatrix = await _unitOfWork.ExamMatrixRepository.GetAsync(x => x.Id == examMatrixId, x => x.Include(ed => ed.ExamMatrixDetail!));
            if (examMatrix != null && examMatrix.ExamMatrixDetail != null)
            {
                var questions = await _questionService.GenerateQuestion(examMatrix.Id);
                if(!questions.IsSuccess || questions.ListResult == null)
                {
                    result.IsSuccess = false;
                    result.Message = questions.Message;
                    result.Result = exam;
                    return result;
                }
                ICollection<ExamDetail> examDetails = new List<ExamDetail>();
                foreach(var q in questions.ListResult)
                {
                    ExamDetail examDetail = new()
                    {
                        Exam = exam,
                        QuestionId = q.Id
                    };
                    examDetails.Add(examDetail);
                }    
                exam.NumberOfQuestion = questions.ListResult.Count;
                exam.Identification = questions.ListResult.Count(x => x.DifficultyLevel == DifficultyLevel.RECOGNITION);
                exam.Understanding = questions.ListResult.Count(x => x.DifficultyLevel == DifficultyLevel.UNDERSTANDING);
                exam.Application = questions.ListResult.Count(x => x.DifficultyLevel == DifficultyLevel.APPLICATION);
                exam.AdvancedApplication = questions.ListResult.Count(x => x.DifficultyLevel == DifficultyLevel.HIGHER_ORDER_APPLICATION);
                exam.IsRemoved = false;
                exam.DateCreate = DateTime.UtcNow.ToTimeZone();
                exam.TotalUserExam = 0;
                await _unitOfWork.ExamRepository.AddAsync(exam);
                await _unitOfWork.ExamDetailRepository.AddRangeAsync(examDetails);
                await _unitOfWork.CommitAsync();
                exam.ExamDetail = examDetails;
                result.IsSuccess = true;
                result.Message = "Sinh đề thành công.";
                result.Result = exam;
                return result;
            }
            result.IsSuccess = false;
            result.Message = "Tài nguyên không đủ để sinh đề.";
            return result;
        }

        public async Task<ICollection<Exam>> GetListAsync(int? ClassId = null, int? SubjectId = null, int? ChapterId = null, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null)
        {
            var exams = await _unitOfWork.ExamRepository.GetAllAsync(null, includes);
            
            var query = exams.AsQueryable();

            if (ClassId.HasValue)
            {
                query = query.Where(e => e.ClassId == ClassId);
            }

            if (SubjectId.HasValue)
            {
                query = query.Where(e => e.SubjectId == SubjectId);
            }

            if (ChapterId.HasValue)
            {
                query = query.Where(e => e.ChapterId == ChapterId);
            }
            return query.ToList();
        }

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

        public async Task<int> CountAsync()
            => await _unitOfWork.ExamRepository.CountAsync();

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
            
            var exam = new Exam()
            {
                ExamType = model.ExamType,
                ExamDescription = model.ExamDescription,
                ExamName = model.ExamName,
                ExamTime = model.ExamTime,
                NumberOfQuestion = questions.Count,
                IsRemoved = false,
                DateCreate = DateTime.UtcNow.ToTimeZone(),
                TotalUserExam = 0,
                SubjectId = model.ExamSubjectId,
                Identification = model.ExamIdentification,
                Understanding = model.ExamUnderstanding,
                Application = model.ExamApplication,
                AdvancedApplication = model.ExamAdvancedApplication,
            };
            ICollection<ExamDetail> examDetails = new List<ExamDetail>();
            foreach (var q in questions)
            {
                ExamDetail examDetail = new()
                {
                    Exam = exam,
                    Question = q
                };
                examDetails.Add(examDetail);
            }
            exam.ExamDetail = examDetails;
            if (model.ExamType == Enums.ExamType.Lop)
            {
                exam.ClassId = model.ExamClassId;
            }
            _unitOfWork.ExamRepository.Add(exam);
            await _unitOfWork.CommitAsync();
        }
    }
}
