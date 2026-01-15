using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.Assuntos
{
    public class DeleteModel : CrudPageModel<Assunto, IAssuntoRepository>
    {
        public DeleteModel(IAssuntoRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        [BindProperty]
        public Assunto Assunto { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assunto = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (assunto == null)
            {
                return NotFound();
            }
            else
            {
                Assunto = assunto;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assunto = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (assunto != null)
            {
                Assunto = assunto;

                _repository.Remove(Assunto);
                await _unitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
