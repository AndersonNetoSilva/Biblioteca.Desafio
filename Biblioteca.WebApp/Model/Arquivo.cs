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

    public class ArquivoVM : IPermiteMarcarParaExclusao
    {
        public int Id { get; set; }
        public string? Descricao { get; set; } = null;

        [Display(Name = "Arquivo")]
        public IFormFile? Arquivo { get; set; }
        public int? ArquivoId { get; set; }
        public bool MarcadoParaExclusao { get; set; } = false;
    }
}
