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

            //[Gustavo Viegas 2026/01/30] 
            //Iniciliza o Grid de Detalhe com uma Linha
            //Opcional mas útil para Detalhes Obrigatórios. Mais fácil de validar.
            Precos.Add(new PrecoDeVendaVM());

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

        //[Gustavo Viegas 2026/01/30] 
        //Lista de Detalhe. Bindable. Usada para Preencher o Grid de Detalhe.
        [BindProperty]
        public List<PrecoDeVendaVM> Precos { get; set; } = new();

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

            //[Gustavo Viegas 2026/01/30] 
            //Percorre a Lista de Detalhe, Atualizar o Valor (por conta da formatação) e dá um ADD na Lista de Detalhe com o que veio da Página.
            //Aqui vai ser sempre ADD pois a página é só de Create.
            foreach (var precoVm in Precos)
            {
                if (!ModelState.TryParseValor("", precoVm.ValorString, out var valor))
                {
                    ModelState.AddModelError("", "Valor de preço inválido.");
                    return Page();
                }

                Livro.PrecosDeVenda.Add(new PrecoDeVenda
                {
                    Tipo = precoVm.Tipo,
                    Valor = valor
                });
            }

            _repository.Add(Livro);

            await _unitOfWork.CommitAsync();

            return RedirectToPage("./Index");
        }
    }
}
