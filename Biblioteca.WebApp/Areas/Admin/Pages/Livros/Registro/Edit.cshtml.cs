using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Abstractions.Services;
using Biblioteca.WebApp.Infrastructure.Exceptions;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.Livros
{
    public class EditModel : CrudPageModel<Livro, ILivroRepository>
    {
        private readonly IAutorRepository _autorRepository;

        private readonly IAssuntoRepository _assuntoRepository;

        private readonly ILivroService _livroService;

        public EditModel(ILivroRepository repository,
            ILivroService livroService,
            IAutorRepository autorRepository,
            IAssuntoRepository assuntoRepository,
            IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _livroService = livroService;

            _autorRepository = autorRepository;
            _assuntoRepository = assuntoRepository;
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

            AutorIds = Livro.Autores.Select(x => x.Id).ToList();

            AssuntoIds = Livro.Assuntos.Select(x => x.Id).ToList();
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

        [BindProperty]
        public List<PrecoDeVendaVM> PrecosDeVenda { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _repository.GetForUpdateAsync(id);

            if (livro == null)
            {
                return NotFound();
            }

            livro.ValorString = livro.Valor.ToString("C", new System.Globalization.CultureInfo("pt-BR"));

            PrecosDeVenda = livro.PrecosDeVenda
                .Select(p => new PrecoDeVendaVM
                {
                    Id = p.Id,
                    Tipo = p.Tipo,
                    ValorString = p.Valor.ToString("N2")
                })
                .ToList();

            Livro = livro;

            BindSelectLists();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                BindSelectLists();
                return Page();
            }

            try
            {
                await _livroService.UpdateAsync(Livro, AutorIds, AssuntoIds, PrecosDeVenda);
            }
            catch (KeyNotFoundException)
            {
                BindSelectLists();
                return NotFound();
            }
            catch (ValidationListException ex)
            {
                BindSelectLists();

                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.Key, error.Value);

                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
