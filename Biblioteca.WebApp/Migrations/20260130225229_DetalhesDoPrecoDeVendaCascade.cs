using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class DetalhesDoPrecoDeVendaCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrecosDeVenda_Livros_LivroId",
                table: "PrecosDeVenda");

            migrationBuilder.AddForeignKey(
                name: "FK_PrecosDeVenda_Livros_LivroId",
                table: "PrecosDeVenda",
                column: "LivroId",
                principalTable: "Livros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrecosDeVenda_Livros_LivroId",
                table: "PrecosDeVenda");

            migrationBuilder.AddForeignKey(
                name: "FK_PrecosDeVenda_Livros_LivroId",
                table: "PrecosDeVenda",
                column: "LivroId",
                principalTable: "Livros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
