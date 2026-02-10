using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.WebApp.Model
{
    public class AnexoDoLivro : EntityBase
    {
        [Required]
        [StringLength(40)]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }

        [Display(Name = "Tipo de Anexo")]
        public TipoANexo? Tipo { get; set; }

        [Display(Name = "Livro")]
        public Livro? Livro { get; set; }

        [Required]
        [ForeignKey(nameof(Livro))]
        [Display(Name = "Livro")]
        public int LivroId { get; set; }

        [ForeignKey(nameof(Anexo))]
        [Display(Name = "Imagem")]
        public int? AnexoId { get; set; }

        [Display(Name = "Imagem")]
        public Arquivo? Anexo { get; set; }
    }

    public class AnexoDoLivroVM : IPermiteMarcarParaExclusao
    {
        public int? Id { get; set; }
        public string? Descricao { get; set; }
        public IFormFile? FormFile { get; set; }
        public bool MarcadoParaExclusao { get; set; } = false;
        public string? Tipo { get; set; }
    }

    public enum TipoANexo
    { 
        Imagem,
        Arquivo
    }
}
