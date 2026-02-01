using Biblioteca.WebApp.Model;
using Biblioteca.WebApp.Model.Views;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Biblioteca.WebApp.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) 
        {

        }

        public DbSet<Assunto> Assuntos { get; set; }

        public DbSet<Autor> Autores { get; set; }

        public DbSet<Livro> Livros { get; set; }

        public DbSet<PrecoDeVenda> PrecosDeVenda { get; set; }

        public DbSet<Arquivo> Arquivos { get; set; }

        public DbSet<ReportLivrosView> ReportLivrosViewSet => Set<ReportLivrosView>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Livro>()
                .Property(c => c.Valor)
                .HasColumnType("decimal")
                .HasPrecision(18, 2);

            builder.Entity<Livro>()
                .HasMany(p => p.Autores)
                .WithMany(p => p.Livros);

            builder.Entity<Livro>()
                .HasMany(p => p.Assuntos)
                .WithMany(p => p.Livros);

            builder.Entity<ReportLivrosView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_ReportLivros");
            });

            builder.Entity<PrecoDeVenda>()
                .HasOne(p => p.Livro)
                .WithMany(l => l.PrecosDeVenda)
                .HasForeignKey(p => p.LivroId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
