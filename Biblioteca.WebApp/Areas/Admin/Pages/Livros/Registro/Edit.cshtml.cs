using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Extensions;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.Livros
{
    public class EditModel : CrudPageModel<Livro, ILivroRepository>
    {
        private readonly IAutorRepository _autorRepository;

        private readonly IAssuntoRepository _assuntoRepository;

        public EditModel(ILivroRepository repository,
            IAutorRepository autorRepository,
            IAssuntoRepository assuntoRepository,
            IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
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

            livro.ValorString = livro.Valor.ToString("C", new System.Globalization.CultureInfo("pt-BR"));

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

            if (!ModelState.TryParseValor("Livro.ValorString", Livro.ValorString, out var valorDecimal))
            {
                BindSelectLists();
                ModelState.AddModelError("Livro.ValorString", "Valor inválido.");
                return Page();
            }

            Livro.Valor = valorDecimal;
            Livro.Autores = _autorRepository.Query().Where(a => AutorIds.Contains(a.Id)).ToList();
            Livro.Assuntos = _assuntoRepository.Query().Where(a => AssuntoIds.Contains(a.Id)).ToList();

            try
            {
                _repository.Update(Livro);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _repository.ExistsAsync(x => x.Id == Livro.Id))
                {
                    BindSelectLists();
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
