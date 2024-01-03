using EduQuiz_5P.Models;
using EduQuiz_5P.Repository.UnitOfWork;
using EduQuiz_5P.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EduQuiz_5P.Services
{
    public class ExamService : IExamService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public ExamService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
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
    }
}
