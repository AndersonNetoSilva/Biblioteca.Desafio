using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.WebApp.Model
{
    public class Livro : EntityBase
    {
        [Required]
        [StringLength(40)]
        [Display(Name = "Título")]
        public required string Titulo { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Editora")]
        public required string Editora { get; set; }

        [Required]
        [Display(Name = "Edição")]
        public required int Edicao { get; set; } = 1;

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Display(Name = "Valor")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Display(Name = "Valor")]
        [NotMapped]
        public string ValorString { get; set; }        

        public List<Autor> Autores { get; set; } = new();
        public List<Assunto> Assuntos { get; set; } = new();
    }
}
