namespace Biblioteca.WebApp.Infrastructure.Abstractions.Repositories
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
