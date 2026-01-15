using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.Livros
{
    public class DeleteModel : CrudPageModel<Livro, ILivroRepository>
    {
        public DeleteModel(ILivroRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (livro != null)
            {
                Livro = livro;

                _repository.Remove(Livro);
                await _unitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
