using Biblioteca.WebApp.Data;
using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Model;

namespace Biblioteca.WebApp.Infrastructure.Repositories
{
    public class AssuntoRepository : DbContextRepository<Assunto, ApplicationDbContext>, IAssuntoRepository
    {
        public AssuntoRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
