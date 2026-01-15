using Biblioteca.WebApp.Data;
using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Model;

namespace Biblioteca.WebApp.Infrastructure.Repositories
{
    public class PrecoDeVendaRepository : DbContextRepository<PrecoDeVenda, ApplicationDbContext>, IPrecoDeVendaRepository
    {
        public PrecoDeVendaRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
