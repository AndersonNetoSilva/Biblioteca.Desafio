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
            //[Gustavo Viegas 2026 / 01 / 30]
            //Faz o Include do Detalhe (PrecosDeVenda)
            var oldLivro = Query()
                            .Where(x => x.Id == entity.Id)
                            .Include(x => x.Autores)
                            .Include(x => x.Assuntos)
                            .Include(x => x.PrecosDeVenda)
                            .FirstOrDefault();

            // Atualiza propriedades simples
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);


            //[Gustavo Viegas 2026 / 01 / 30] 
            //Atualiza Propriedades
            oldLivro.Titulo = entity.Titulo;
            oldLivro.Editora = entity.Editora;
            oldLivro.Edicao = entity.Edicao;
            oldLivro.Valor = entity.Valor;

            // Atualiza as coleções (limpa e adiciona as novas referências)
            oldLivro.Autores.Clear();
            oldLivro.Autores.AddRange(entity.Autores);

            oldLivro.Assuntos.Clear();
            oldLivro.Assuntos.AddRange(entity.Assuntos);

            oldLivro.PrecosDeVenda.Clear();
            oldLivro.PrecosDeVenda.AddRange(entity.PrecosDeVenda);

            base.Update(oldLivro);

            entity = oldLivro;
        }
    }
}
