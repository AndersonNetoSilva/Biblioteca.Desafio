using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.WebApp.Model
{
    public class PrecoDeVenda : EntityBase
    {
        [Display(Name = "Livro")]
        public Livro? Livro { get; set; }


        //[Gustavo Viegas 2026/01/30]
        //No modelo de Detalhe o Identificador do mestre não pode ser required
        [Required]
        [ForeignKey(nameof(Livro))]
        [Display(Name = "Livro")]
        public int LivroId { get; set; }
        public required TipoDeVenda Tipo { get; set; } = TipoDeVenda.Balcao;

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Display(Name = "Valor")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Display(Name = "Valor")]
        [NotMapped]
        public string ValorString { get; set; }
    }

    public enum TipoDeVenda
    {
        [Description("Balcão")]
        Balcao = 0,
        [Description("Self-service")]
        SelfService = 1,
        [Description("Internet")]
        Internet = 2,
        [Description("Evento")]
        Evento = 3,
        [Description("Revenda")]
        Revenda = 4,
        [Description("Outros")]
        Outros = 99
    }

    public class PrecoDeVendaVM
    {
        public int? Id { get; set; }
        public TipoDeVenda Tipo { get; set; }
        public string ValorString { get; set; }
    }
}
