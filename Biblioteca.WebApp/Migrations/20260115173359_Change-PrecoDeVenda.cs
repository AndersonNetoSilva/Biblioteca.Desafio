using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangePrecoDeVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrecosDeVenda_Livros_LivroId",
                table: "PrecosDeVenda");

            migrationBuilder.RenameColumn(
                name: "LivroId",
                table: "PrecosDeVenda",
                newName: "LivroId1");

            migrationBuilder.RenameIndex(
                name: "IX_PrecosDeVenda_LivroId",
                table: "PrecosDeVenda",
                newName: "IX_PrecosDeVenda_LivroId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PrecosDeVenda_Livros_LivroId1",
                table: "PrecosDeVenda",
                column: "LivroId1",
                principalTable: "Livros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrecosDeVenda_Livros_LivroId1",
                table: "PrecosDeVenda");

            migrationBuilder.RenameColumn(
                name: "LivroId1",
                table: "PrecosDeVenda",
                newName: "LivroId");

            migrationBuilder.RenameIndex(
                name: "IX_PrecosDeVenda_LivroId1",
                table: "PrecosDeVenda",
                newName: "IX_PrecosDeVenda_LivroId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrecosDeVenda_Livros_LivroId",
                table: "PrecosDeVenda",
                column: "LivroId",
                principalTable: "Livros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
