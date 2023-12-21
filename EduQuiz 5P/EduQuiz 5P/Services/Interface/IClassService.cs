using EduQuiz_5P.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace EduQuiz_5P.Services.Interface
{
    public interface IClassService
    {
        public Task<ICollection<Classes>> GetListAsync();
        public Task<ICollection<Classes>> GetListAsyncWithIncludes(Func<IQueryable<Classes>, IIncludableQueryable<Classes, object>> includes, int? Id = null, string? Name = null);
        public Task Add(Classes @class, long userId);
        public Task AddRange(ICollection<Classes> model, long userId);
        public Task<Classes?> GetByIdAsync(int? id);
        public Task<Classes?> GetByIdAsync(int? id, Func<IQueryable<Classes>, IIncludableQueryable<Classes, object>> includes);
        public Classes? GetById(int id);
        public Task<bool> Delete(int Id);
        public Task Update(Classes @class);
    }
}
