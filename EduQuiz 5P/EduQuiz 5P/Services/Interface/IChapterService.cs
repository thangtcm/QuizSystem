using EduQuiz_5P.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace EduQuiz_5P.Services.Interface
{
    public interface IChapterService
    {
        public Task<ICollection<Chapter>> GetListAsync(int? classId = null, int? subjectId = null, Func<IQueryable<Chapter>, IIncludableQueryable<Chapter, object>>? includes = null);
        public Task<ICollection<Chapter>> GetListAsync();
        public Task<ICollection<Chapter>> GetListAsyncWithIncludes(Func<IQueryable<Chapter>, IIncludableQueryable<Chapter, object>> includes);
        public Task Add(Chapter chapter);
        public Task AddRange(ICollection<Chapter> chapters, long userId);
        public Task<Chapter?> GetByIdAsync(int? id);
        public Task<Chapter?> GetByIdAsync(int? id, Func<IQueryable<Chapter>, IIncludableQueryable<Chapter, object>> includes);
        public Chapter? Find(int? id);
        public Task<Chapter?> FindAsync(int? id);
        public Chapter? GetById(int? id);
        public Task<bool> Delete(int Id);
        public Task Update(Chapter chapter);
        public Task SaveChanges();
    }
}
