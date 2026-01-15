using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Extensions;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.PrecosDeVenda
{
    public class CreateModel : CrudPageModel<PrecoDeVenda, IPrecoDeVendaRepository>
    {
        private readonly ILivroRepository _livroRepository;

        public CreateModel(IPrecoDeVendaRepository repository,
            ILivroRepository livroRepository,
            IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _livroRepository = livroRepository;
        }

        public IActionResult OnGet()
        {
            BindSelectLists();

            return Page();
        }

        #region SelectLists

        private void BindSelectLists()
        {
            ViewData["Livros"] = _livroRepository.Query()
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Titulo })
                .OrderBy(a => a.Text)
                .ToList();
        }

        #endregion

        [BindProperty]
        public PrecoDeVenda PrecoDeVenda { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            BindSelectLists();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!ModelState.TryParseValor("PrecoDeVenda.ValorString", PrecoDeVenda.ValorString, out var valorDecimal))
            {
                ModelState.AddModelError("PrecoDeVenda.ValorString", "Valor inválido.");
                return Page();
            }

            PrecoDeVenda.Valor = valorDecimal;
            PrecoDeVenda.Livro = _livroRepository.Query().FirstOrDefault(x => x.Id == PrecoDeVenda.LivroId);

            _repository.Add(PrecoDeVenda);

            await _unitOfWork.CommitAsync();

            return RedirectToPage("./Index");
        }
    }
}
