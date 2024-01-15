using EduQuiz_5P.Models;
using EduQuiz_5P.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EduQuiz_5P.Services.Interface
{
    public interface IExamService
    {
        public Task<ICollection<Exam>> GetExamDefaultList(int? ClassId = null, int? SubjectId = null, int? ChapterId = null, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null);
        public Task<ICollection<Exam>> GetExamOwnerList(long userId, int? ClassId = null, int? SubjectId = null, int? ChapterId = null, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null);
        public Task<ResponResultData<Exam>> CreateExamWithMatrix(Exam exam, int examMatrixId, long userId);
        public Task CreateExamImport(ImportExamFileVM model);
        public Task<ICollection<Exam>> GetListAsync(int? ClassId = null, int? SubjectId = null, int? ChapterId = null, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null);
        public Task<Exam?> GetByIdAsync(int Id, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>>? includes = null);
        public Exam? GetById(int id);
        public Task<Exam?> GetByIdAsync(int Id);
        public Task DeleteExam(int examId);
        public Task<int> CountAsync();
    }
}
