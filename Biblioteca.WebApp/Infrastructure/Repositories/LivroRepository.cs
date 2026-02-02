using Biblioteca.WebApp.Data;
using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Infrastructure.Repositories
{
    public class LivroRepository : DbContextRepository<Livro, ApplicationDbContext>, ILivroRepository
    {
        public LivroRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<Livro?> GetForUpdateAsync(int? id)
            
        {
            return await Query()
                .Include(x => x.Autores)
                .Include(x => x.Assuntos)
                .Include(x => x.PrecosDeVenda)
                .Include(x => x.ArquivoImagem)
                .Include(x => x.ArquivoDownload)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
