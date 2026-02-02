using System.ComponentModel.DataAnnotations;

namespace Biblioteca.WebApp.Model
{
    public class Arquivo: EntityBase
    {
        public string NomeOriginal { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] Conteudo { get; set; } = null!;
        public int Tamanho { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataUltimaAlteracao { get; set; } = null;
    }

    public class ArquivoVM 
    {
        public int Id { get; set; }
        public string? Descricao { get; set; } = null;
        public IFormFile? FormFile { get; set; }
    }
}
