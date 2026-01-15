using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.Livros
{
    public class DetailsModel : CrudPageModel<Livro, ILivroRepository>
    {
        public DetailsModel(ILivroRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public Livro Livro { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _repository
                                .Query()
                                .Where(x => x.Id == id.Value)
                                .Include(x => x.Autores)
                                .Include(x => x.Assuntos)
                                .FirstOrDefaultAsync();

            if (livro == null)
            {
                return NotFound();
            }
            else
            {
                Livro = livro;
            }

            return Page();
        }
    }
}
