using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Infrastructure.Repositories
{
    public sealed class UnitOfWork<T> : IUnitOfWork
         where T : DbContext
    {
        private readonly T _dbContext;

        public UnitOfWork(T dbContext)
        {
            _dbContext = dbContext
                ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
