using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Extensions;
using Biblioteca.WebApp.Infrastructure.Pages;
using Biblioteca.WebApp.Infrastructure.Repositories;
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

        //[Gustavo Viegas 2026/01/30] 
        //Atributo Privado para o Repositório do Detalhe pra Poder Recuperar a Lista com Mais Facilidade ao Gravar.
        //Tem outras formas de Fazer, esse é bem Prática
        private readonly IPrecoDeVendaRepository _precoDeVendaRepository;


        //[Gustavo Viegas 2026/01/30] 
        //Injeta o Repositório do Detalhe pra Poder Recuperar a Lista com Mais Facilidade ao Gravar.       
        public EditModel(ILivroRepository repository,
            IAutorRepository autorRepository,
            IAssuntoRepository assuntoRepository,
            IPrecoDeVendaRepository precoDeVendaRepository,
            IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _autorRepository = autorRepository;
            _assuntoRepository = assuntoRepository;

            //[Gustavo Viegas 2026/01/30] 
            //Injeta o Repositório do Detalhe pra Poder Recuperar a Lista com Mais Facilidade ao Gravar.       
            _precoDeVendaRepository = precoDeVendaRepository;   
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

        //[Gustavo Viegas 2026/01/30] 
        //Lista de Detalhe. Bindable. Usada para Preencher o Grid de Detalhe.
        [BindProperty]
        public List<PrecoDeVendaVM> Precos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //[Gustavo Viegas 2026/01/30]
            //Include da Lista de Detalhe pra Poder Preencher o Grid
            var livro = await _repository
                                .Query()
                                .Where(x => x.Id == id.Value)
                                .Include(x => x.Autores)
                                .Include(x => x.Assuntos)
                                .Include(x => x.PrecosDeVenda)
                                .FirstOrDefaultAsync();

            if (livro == null)
            {
                return NotFound();
            }

            livro.ValorString = livro.Valor.ToString("C", new System.Globalization.CultureInfo("pt-BR"));

            //[Gustavo Viegas 2026/01/30]
            //Popula o Atributo da Lista de Detalhe com o que está Gravado na Entidade.
            //Esse atributo Precos é usado para Preencher o Grid e Pegar os Valores Editados de Volta
            Precos = livro.PrecosDeVenda
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

            if (!ModelState.TryParseValor("Livro.ValorString", Livro.ValorString, out var valorDecimal))
            {
                BindSelectLists();
                ModelState.AddModelError("Livro.ValorString", "Valor inválido.");
                return Page();
            }

            Livro.Valor = valorDecimal;
            Livro.Autores = _autorRepository.Query().Where(a => AutorIds.Contains(a.Id)).ToList();
            Livro.Assuntos = _assuntoRepository.Query().Where(a => AssuntoIds.Contains(a.Id)).ToList();

            //[Gustavo Viegas 2026/01/30]
            //Recupera os Detalhes já Gravados
            Livro.PrecosDeVenda = _precoDeVendaRepository
                                    .Query()
                                    .Where(x => x.LivroId == Livro.Id)
                                    .ToList();

            //[Gustavo Viegas 2026/01/30]
            //Pega o que veio da Página
            var idsPostados = Precos.Where(p => p.Id.HasValue).Select(p => p.Id.Value).ToList();


            //[Gustavo Viegas 2026/01/30]
            //Exclui o que não está na Página
            Livro.PrecosDeVenda
                .RemoveAll(p => !idsPostados.Contains(p.Id));

            //[Gustavo Viegas 2026/01/30]
            //Percorre a partir do que veio da Página, inclui ou alterada para que seja gravado o que está na página
            foreach (var vm in Precos)
            {
                if (!ModelState.TryParseValor("", vm.ValorString, out var valor))
                    continue;

                var preco = Livro.PrecosDeVenda.FirstOrDefault(p => p.Id == (vm.Id ?? 0));

                if (preco != null)
                {
                    preco.Tipo = vm.Tipo;
                    preco.Valor = valor;
                }
                else
                {
                    Livro.PrecosDeVenda.Add(new PrecoDeVenda
                    {
                        Tipo = vm.Tipo,
                        Valor = valor
                    });
                }
            }

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
