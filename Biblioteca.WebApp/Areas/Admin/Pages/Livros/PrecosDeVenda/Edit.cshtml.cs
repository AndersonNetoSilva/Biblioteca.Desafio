using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Extensions;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.PrecosDeVenda
{
    public class EditModel : CrudPageModel<PrecoDeVenda, IPrecoDeVendaRepository>
    {
        private readonly ILivroRepository _livroRepository;

        public EditModel(IPrecoDeVendaRepository repository,
            ILivroRepository livroRepository,
            IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _livroRepository = livroRepository;
        }

        #region SelectLists

        private void BindSelectLists()
        {
            ViewData["Livros"] = _livroRepository.Query()
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Titulo })
                .OrderBy(a => a.Text)
                .ToList();

            PrecoDeVenda.LivroId = PrecoDeVenda?.LivroId ?? 0;
        }

        #endregion

        [BindProperty]
        public PrecoDeVenda PrecoDeVenda { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var precoDeVenda = await _repository
                                .Query()
                                .Where(x => x.Id == id.Value)
                                .Include(x => x.Livro)
                                .FirstOrDefaultAsync();

            if (precoDeVenda == null)
            {
                return NotFound();
            }

            precoDeVenda.ValorString = precoDeVenda.Valor.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
            precoDeVenda.Livro = _livroRepository.Query().FirstOrDefault(x => x.Id == precoDeVenda.LivroId);

            PrecoDeVenda = precoDeVenda;

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

            if (!ModelState.TryParseValor("PrecoDeVenda.ValorString", PrecoDeVenda.ValorString, out var valorDecimal))
            {
                BindSelectLists();
                ModelState.AddModelError("PrecoDeVenda.ValorString", "Valor inválido.");
                return Page();
            }

            PrecoDeVenda.Valor = valorDecimal;
            PrecoDeVenda.Livro = _livroRepository.Query().FirstOrDefault(x => x.Id == PrecoDeVenda.LivroId);

            try
            {
                _repository.Update(PrecoDeVenda);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _repository.ExistsAsync(x => x.Id == PrecoDeVenda.Id))
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
