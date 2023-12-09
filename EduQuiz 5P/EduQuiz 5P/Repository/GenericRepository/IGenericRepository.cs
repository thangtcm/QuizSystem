using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EduQuiz_5P.Repository.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        T? Get(Expression<Func<T, bool>> expression);
        T? Find(object Id);
        Task<T?> FindAsync(object Id);
        ICollection<T> GetAll();
        ICollection<T> GetAll(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(ICollection<T> entities);
        void Remove(T entity);
        void RemoveRange(ICollection<T> entities);
        void Update(T entity);
        void UpdateRange(ICollection<T> entities);
        Task<T?> GetAsync(Expression<Func<T, bool>>? expression, Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken cancellationToken = default);
        Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? expression,
                                             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                             int? take = null,
                                             CancellationToken cancellationToken = default);
        Task<ICollection<T>> GetRandomItemsAsync(int numberOfItems, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
        int Count(Expression<Func<T, bool>> expression);
        SelectList GetSelectList(string Id, string Name);
    }
}
