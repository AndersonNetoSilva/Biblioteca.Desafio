using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class ArquivoDownload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Arquivos_ArquivoCapaId",
                table: "Livros");

            migrationBuilder.RenameColumn(
                name: "ArquivoCapaId",
                table: "Livros",
                newName: "ArquivoImagemId");

            migrationBuilder.RenameIndex(
                name: "IX_Livros_ArquivoCapaId",
                table: "Livros",
                newName: "IX_Livros_ArquivoImagemId");

            migrationBuilder.AddColumn<int>(
                name: "ArquivoDownloadId",
                table: "Livros",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Livros_ArquivoDownloadId",
                table: "Livros",
                column: "ArquivoDownloadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Arquivos_ArquivoDownloadId",
                table: "Livros",
                column: "ArquivoDownloadId",
                principalTable: "Arquivos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Arquivos_ArquivoImagemId",
                table: "Livros",
                column: "ArquivoImagemId",
                principalTable: "Arquivos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Arquivos_ArquivoDownloadId",
                table: "Livros");

            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Arquivos_ArquivoImagemId",
                table: "Livros");

            migrationBuilder.DropIndex(
                name: "IX_Livros_ArquivoDownloadId",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "ArquivoDownloadId",
                table: "Livros");

            migrationBuilder.RenameColumn(
                name: "ArquivoImagemId",
                table: "Livros",
                newName: "ArquivoCapaId");

            migrationBuilder.RenameIndex(
                name: "IX_Livros_ArquivoImagemId",
                table: "Livros",
                newName: "IX_Livros_ArquivoCapaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Arquivos_ArquivoCapaId",
                table: "Livros",
                column: "ArquivoCapaId",
                principalTable: "Arquivos",
                principalColumn: "Id");
        }
    }
}
