using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using EduQuiz_5P.Data;

namespace EduQuiz_5P.Repository.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _entitySet;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _entitySet = _dbContext.Set<T>();
        }

        public void Add(T entity)
            => _dbContext.Add(entity);


        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await _dbContext.AddAsync(entity, cancellationToken);


        public void AddRange(ICollection<T> entities)
            => _entitySet.AddRange(entities);


        public async Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
            => await _entitySet.AddRangeAsync(entities, cancellationToken);


        public T? Get(Expression<Func<T, bool>> expression)
            => _entitySet.FirstOrDefault(expression);

        public T? Find(object Id)
            => _entitySet.Find(Id);

        public async Task<T?> FindAsync(object Id)
            => await _entitySet.FindAsync(Id);

        public ICollection<T> GetAll()
            => _entitySet.ToList();


        public ICollection<T> GetAll(Expression<Func<T, bool>> expression)
            => _entitySet.AsNoTracking().Where(expression).ToList();

        public async Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _entitySet.AsNoTracking().ToListAsync(cancellationToken);


        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? expression,
                                             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                             int? take = null,
                                             CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _entitySet.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (take.HasValue && take.Value > 0)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<ICollection<T>> GetRandomItemsAsync(int numberOfItems, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _entitySet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.OrderBy(q => Guid.NewGuid()).Take(numberOfItems).ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>>? expression, Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _entitySet;

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(expression!, cancellationToken);
        }


        public void Remove(T entity)
            => _dbContext.Remove(entity);


        public void RemoveRange(ICollection<T> entities)
            => _dbContext.RemoveRange(entities);

        public void Update(T entity)
            => _entitySet.Update(entity);

        public void UpdateRange(ICollection<T> entities)
            => _entitySet.UpdateRange(entities);

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
            => await _entitySet.CountAsync(expression);

        public int Count(Expression<Func<T, bool>> expression)
            => _entitySet.Count(expression);

        public SelectList GetSelectList(string Id, string Name)
        {
            var items = _entitySet.ToList();
            var selectList = new SelectList(items, Id, Name);

            return selectList;
        }
    }
}
