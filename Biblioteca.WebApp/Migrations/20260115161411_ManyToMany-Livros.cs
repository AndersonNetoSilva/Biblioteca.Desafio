using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyLivros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assuntos_Livros_LivroId",
                table: "Assuntos");

            migrationBuilder.DropForeignKey(
                name: "FK_Autores_Livros_LivroId",
                table: "Autores");

            migrationBuilder.DropIndex(
                name: "IX_Autores_LivroId",
                table: "Autores");

            migrationBuilder.DropIndex(
                name: "IX_Assuntos_LivroId",
                table: "Assuntos");

            migrationBuilder.DropColumn(
                name: "LivroId",
                table: "Autores");

            migrationBuilder.DropColumn(
                name: "LivroId",
                table: "Assuntos");

            migrationBuilder.CreateTable(
                name: "AssuntoLivro",
                columns: table => new
                {
                    AssuntosId = table.Column<int>(type: "int", nullable: false),
                    LivrosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssuntoLivro", x => new { x.AssuntosId, x.LivrosId });
                    table.ForeignKey(
                        name: "FK_AssuntoLivro_Assuntos_AssuntosId",
                        column: x => x.AssuntosId,
                        principalTable: "Assuntos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssuntoLivro_Livros_LivrosId",
                        column: x => x.LivrosId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutorLivro",
                columns: table => new
                {
                    AutoresId = table.Column<int>(type: "int", nullable: false),
                    LivrosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorLivro", x => new { x.AutoresId, x.LivrosId });
                    table.ForeignKey(
                        name: "FK_AutorLivro_Autores_AutoresId",
                        column: x => x.AutoresId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutorLivro_Livros_LivrosId",
                        column: x => x.LivrosId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssuntoLivro_LivrosId",
                table: "AssuntoLivro",
                column: "LivrosId");

            migrationBuilder.CreateIndex(
                name: "IX_AutorLivro_LivrosId",
                table: "AutorLivro",
                column: "LivrosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssuntoLivro");

            migrationBuilder.DropTable(
                name: "AutorLivro");

            migrationBuilder.AddColumn<int>(
                name: "LivroId",
                table: "Autores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LivroId",
                table: "Assuntos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Autores_LivroId",
                table: "Autores",
                column: "LivroId");

            migrationBuilder.CreateIndex(
                name: "IX_Assuntos_LivroId",
                table: "Assuntos",
                column: "LivroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assuntos_Livros_LivroId",
                table: "Assuntos",
                column: "LivroId",
                principalTable: "Livros",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Autores_Livros_LivroId",
                table: "Autores",
                column: "LivroId",
                principalTable: "Livros",
                principalColumn: "Id");
        }
    }
}
