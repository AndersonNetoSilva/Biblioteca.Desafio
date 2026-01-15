using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Biblioteca.WebApp.Infrastructure.Repositories
{
    public abstract class DbContextRepository<T, TContext> : IRepository<T>
        where T : EntityBase
        where TContext : DbContext
    {
        protected readonly TContext _dbContext;

        public DbContextRepository(TContext dbContext)
        {
            _dbContext = dbContext
                ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual void Add(T entity) => _dbContext.Set<T>().Add(entity);

        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entry = _dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                _dbContext.Set<T>().Attach(entity);
                entry.State = EntityState.Modified;
            }
            else
            {
                entry.State = EntityState.Modified;
            }
        }

        public virtual void Remove(T entity) => _dbContext.Set<T>().Remove(entity);

        public virtual IQueryable<T> Query() => _dbContext.Set<T>();

        public virtual async Task<T?> GetByIdAsync(int id) =>
            await _dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

        public virtual async Task<List<T>> SearchAsync(Expression<Func<T, bool>> predicate) =>
            await _dbContext.Set<T>()
                .Where(predicate)
                .ToListAsync();

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await _dbContext.Set<T>().AnyAsync(predicate);
        }
    }
}
