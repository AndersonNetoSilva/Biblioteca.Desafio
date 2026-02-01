using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class CapaDoLivro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArquivoCapaId",
                table: "Livros",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Arquivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeOriginal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conteudo = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Tamanho = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arquivo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livros_ArquivoCapaId",
                table: "Livros",
                column: "ArquivoCapaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Arquivo_ArquivoCapaId",
                table: "Livros",
                column: "ArquivoCapaId",
                principalTable: "Arquivo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Arquivo_ArquivoCapaId",
                table: "Livros");

            migrationBuilder.DropTable(
                name: "Arquivo");

            migrationBuilder.DropIndex(
                name: "IX_Livros_ArquivoCapaId",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "ArquivoCapaId",
                table: "Livros");
        }
    }
}
