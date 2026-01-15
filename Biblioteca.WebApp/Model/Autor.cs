using System.ComponentModel.DataAnnotations;

namespace Biblioteca.WebApp.Model
{
    public class Autor : EntityBase
    {
        [Required]
        [StringLength(40)]
        [Display(Name = "Nome")]
        public required string Nome { get; set; }

        public List<Livro> Livros { get; set; } = new();
    }
}
