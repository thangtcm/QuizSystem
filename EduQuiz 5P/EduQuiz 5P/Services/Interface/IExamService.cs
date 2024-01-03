using EduQuiz_5P.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EduQuiz_5P.Services.Interface
{
    public interface IExamService
    {
        public Task<ICollection<Exam>> GetExamDefaultList(int? ClassId = null, int? SubjectId = null, int? ChapterId = null, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null);
        public Task<ICollection<Exam>> GetExamOwnerList(long userId, int? ClassId = null, int? SubjectId = null, int? ChapterId = null, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null);
        public Task CreateExamWithMatrix(Exam exam, int examMatrixId);
        public Task<Exam?> CreateExamMatrixDefault(long userId);
        public Task<Exam> CreateExam(int NumberOfQuestions, int ExamTime, int? ClassId = null, int? SubjectId = null);
        public Task<Exam?> GetByIdAsync(int Id, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null);
        public Exam? GetById(int id);
        public Task<Exam?> GetByIdAsync(int Id);
        public Task DeleteExam(int examId);
    }
}
