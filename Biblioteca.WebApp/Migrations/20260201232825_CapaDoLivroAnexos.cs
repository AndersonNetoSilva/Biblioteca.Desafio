using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class CapaDoLivroAnexos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Arquivo_ArquivoCapaId",
                table: "Livros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Arquivo",
                table: "Arquivo");

            migrationBuilder.RenameTable(
                name: "Arquivo",
                newName: "Arquivos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Arquivos",
                table: "Arquivos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Arquivos_ArquivoCapaId",
                table: "Livros",
                column: "ArquivoCapaId",
                principalTable: "Arquivos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Arquivos_ArquivoCapaId",
                table: "Livros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Arquivos",
                table: "Arquivos");

            migrationBuilder.RenameTable(
                name: "Arquivos",
                newName: "Arquivo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Arquivo",
                table: "Arquivo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Arquivo_ArquivoCapaId",
                table: "Livros",
                column: "ArquivoCapaId",
                principalTable: "Arquivo",
                principalColumn: "Id");
        }
    }
}
