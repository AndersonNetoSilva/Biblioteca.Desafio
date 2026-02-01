using Biblioteca.WebApp.Model;

namespace Biblioteca.WebApp.Infrastructure.Abstractions.Repositories
{
    public interface ILivroRepository : IRepository<Livro>
    {
        Task<Livro?> GetForUpdateAsync(int? id);
    }
}
