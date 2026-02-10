using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AnexosDoLivroMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnexosDoLivro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: true),
                    LivroId = table.Column<int>(type: "int", nullable: false),
                    AnexoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnexosDoLivro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnexosDoLivro_Arquivos_AnexoId",
                        column: x => x.AnexoId,
                        principalTable: "Arquivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnexosDoLivro_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnexosDoLivro_AnexoId",
                table: "AnexosDoLivro",
                column: "AnexoId");

            migrationBuilder.CreateIndex(
                name: "IX_AnexosDoLivro_LivroId",
                table: "AnexosDoLivro",
                column: "LivroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnexosDoLivro");
        }
    }
}
