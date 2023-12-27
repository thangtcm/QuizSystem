using EduQuiz_5P.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace EduQuiz_5P.Services.Interface
{
    public interface ISubjectService
    {
        public Task<ICollection<Subject>> GetListAsync(int? classId = null, Func<IQueryable<Subject>, IIncludableQueryable<Subject, object>>? includes = null);
        public Task Add(Subject subject, long userId);
        public Task AddRange(ICollection<Subject> subjects, long userId);
        public Task<Subject?> GetByIdAsync(int? id);
        public Task<Subject?> GetByIdAsync(int? id, Func<IQueryable<Subject>, IIncludableQueryable<Subject, object>> includes);
        public Subject? GetById(int id);
        public Task<bool> Delete(int Id);
        public Task Update(Subject subject);
    }
}
