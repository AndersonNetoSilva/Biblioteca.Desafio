using Biblioteca.WebApp.Data;
using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Model;

namespace Biblioteca.WebApp.Infrastructure.Repositories
{
    public class AutorRepository : DbContextRepository<Autor, ApplicationDbContext>, IAutorRepository
    {
        public AutorRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
