using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Abstractions.Services;
using Biblioteca.WebApp.Infrastructure.Exceptions;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.Livros
{
    public class CreateModel : CrudPageModel<Livro, ILivroRepository>
    {
        private readonly IAutorRepository _autorRepository;

        private readonly IAssuntoRepository _assuntoRepository;

        private readonly ILivroService _livroService;

        public CreateModel(ILivroRepository repository,
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

        public IActionResult OnGet()
        {
            BindSelectLists();

            return Page();
        }

        public async Task<IActionResult> OnGetConteudoDoArquivoDaCapaAsync(int livroId)
        {
            var livro = await _repository.Query()
                .Include(x => x.ArquivoCapa)
                .FirstOrDefaultAsync(x => x.Id == livroId);

            if (livro?.ArquivoCapa == null)
                return NotFound();

            return File(
                livro.ArquivoCapa.Conteudo,
                livro.ArquivoCapa.ContentType
            );
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

        [BindProperty]
        public List<PrecoDeVendaVM> PrecosDeVenda { get; set; } = new();

        [BindProperty]
        public ArquivoVM FotoDaCapa { get; set; } = new();

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
                await _livroService.AddAsync(Livro, AutorIds, AssuntoIds, PrecosDeVenda, FotoDaCapa);
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
