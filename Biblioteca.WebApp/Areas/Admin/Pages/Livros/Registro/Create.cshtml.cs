using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Extensions;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.Livros
{
    public class CreateModel : CrudPageModel<Livro, ILivroRepository>
    {
        private readonly IAutorRepository _autorRepository;

        private readonly IAssuntoRepository _assuntoRepository;

        public CreateModel(ILivroRepository repository,
            IAutorRepository autorRepository,
            IAssuntoRepository assuntoRepository,
            IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _autorRepository = autorRepository;
            _assuntoRepository = assuntoRepository;
        }

        public IActionResult OnGet()
        {
            BindSelectLists();

            return Page();
        }

        #region SelectLists

        private void BindSelectLists()
        {
            Autores = _autorRepository.Query()
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nome })
                .OrderBy(a => a.Text)
                .ToList();

            Assuntos = _assuntoRepository.Query()
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Descricao })
                .OrderBy(a => a.Text)
                .ToList();
        }

        public List<SelectListItem> Autores { get; set; } = new();
        public List<SelectListItem> Assuntos { get; set; } = new();

        [BindProperty]
        public List<int> AutorIds { get; set; } = new();

        [BindProperty]
        public List<int> AssuntoIds { get; set; } = new();

        #endregion

        [BindProperty]
        public Livro Livro { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            BindSelectLists();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!ModelState.TryParseValor("Livro.ValorString", Livro.ValorString, out var valorDecimal))
            {
                ModelState.AddModelError("Livro.ValorString", "Valor inválido.");
                return Page();
            }

            Livro.Valor = valorDecimal;
            Livro.Autores = _autorRepository.Query().Where(a => AutorIds.Contains(a.Id)).ToList();
            Livro.Assuntos = _assuntoRepository.Query().Where(a => AssuntoIds.Contains(a.Id)).ToList();

            _repository.Add(Livro);

            await _unitOfWork.CommitAsync();

            return RedirectToPage("./Index");
        }
    }
}
