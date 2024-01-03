using EduQuiz_5P.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace EduQuiz_5P.Services.Interface
{
    public interface IExamMatrixService
    {
        public Task<ICollection<ExamMatrix>> GetListAsync();
        public Task<ICollection<ExamMatrix>> GetListAsync(Func<IQueryable<ExamMatrix>, IIncludableQueryable<ExamMatrix, object>> includes, string? Name = null);
        public Task Add(ExamMatrix examMatrix);
        public Task<ExamMatrix> GetByIdAsync(int id);
        public Task<ExamMatrix> GetByIdAsync(int id, Func<IQueryable<ExamMatrix>, IIncludableQueryable<ExamMatrix, object>> includes);
        public ExamMatrix GetById(int id);
        public Task<bool> Delete(int Id);
        public Task Update(ExamMatrix examMatrix);
    }
}
