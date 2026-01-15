using System.ComponentModel.DataAnnotations;

namespace Biblioteca.WebApp.Model
{
    public class Assunto : EntityBase
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }

        public List<Livro> Livros { get; set; } = new();
    }
}
