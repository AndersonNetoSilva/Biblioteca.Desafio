using System.ComponentModel.DataAnnotations;

namespace Biblioteca.WebApp.Model
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
