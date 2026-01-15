using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateLivrosReportView : Migration
    {
        private readonly string viewName = "vw_ReportLivros";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
                    CREATE VIEW {viewName} AS
                    SELECT 
	                    Autores.Nome as Autor,
                        Autores.Id as AutorId,
	                    Livros.Titulo as Livro,
                        Livros.Id as LivroId,
	                    Assuntos.Descricao as Assunto,
	                    Assuntos.Id as AssuntoId	
                    FROM
	                    Livros,	
	                    Autores, 
	                    Assuntos, 
	                    AssuntoLivro, 
	                    AutorLivro
                    WHERE 
	                    Livros.Id = AutorLivro.LivrosId and
	                    Autores.Id = AutorLivro.AutoresId and
	                    Livros.Id = AssuntoLivro.LivrosId and
	                    Assuntos.Id = AssuntoLivro.AssuntosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"DROP VIEW {viewName}");
        }
    }
}
