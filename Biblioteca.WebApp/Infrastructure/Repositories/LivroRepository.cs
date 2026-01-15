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

        public override void Update(Livro entity)
        {
            var oldLivro = Query()
                            .Where(x => x.Id == entity.Id)
                            .Include(x => x.Autores)
                            .Include(x => x.Assuntos)
                            .FirstOrDefault();

            // Atualiza propriedades simples
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);

            // Atualiza as coleções (limpa e adiciona as novas referências)
            oldLivro.Autores.Clear();
            oldLivro.Autores.AddRange(entity.Autores);

            oldLivro.Assuntos.Clear();
            oldLivro.Assuntos.AddRange(entity.Assuntos);

            base.Update(oldLivro);

            entity = oldLivro;
        }
    }
}
