using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.PrecosDeVenda
{
    public class DeleteModel : CrudPageModel<PrecoDeVenda, IPrecoDeVendaRepository>
    {
        public DeleteModel(IPrecoDeVendaRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        [BindProperty]
        public PrecoDeVenda PrecoDeVenda { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _repository
                                .Query()
                                .Where(x => x.Id == id.Value)
                                .Include(x => x.Livro)
                                .FirstOrDefaultAsync();

            if (livro == null)
            {
                return NotFound();
            }
            else
            {
                PrecoDeVenda = livro;
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
                PrecoDeVenda = livro;

                _repository.Remove(PrecoDeVenda);
                await _unitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
