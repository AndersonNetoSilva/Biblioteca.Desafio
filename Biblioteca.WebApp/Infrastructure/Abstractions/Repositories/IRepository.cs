using Biblioteca.WebApp.Model;
using System.Linq.Expressions;

namespace Biblioteca.WebApp.Infrastructure.Abstractions.Repositories
{
    public interface IRepository<T>
        where T: EntityBase
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> Query();
        Task<List<T>> SearchAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
