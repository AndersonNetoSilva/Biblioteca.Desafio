using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Exceptions;
using Biblioteca.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.Livros
{
    public class LocacaoModel : PageModel
    {
        protected readonly ILivroRepository _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public LocacaoModel(ILivroRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public LocacaoVM Locacao { get; set; } = default!;

        private async Task BindSelectListsAsync()
        {
            ViewData["Livros"] = await _repository.Query()
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Titulo })
                .OrderBy(a => a.Text)
                .ToListAsync();
        }

        public async Task<IActionResult> OnGetFillRegistrosDeLocacaoAsync(int livroId,
            DateTime? dataDeSaidaInicial = null, DateTime? dataDeDevolucaoInicial = null)
        {
            var registros = await FiltrarRegistrosAsync(livroId, dataDeSaidaInicial, dataDeDevolucaoInicial);

            return Partial("_RegistrosDeLocacaoGrid", registros);
        }

        public async Task<IActionResult> OnGetNewRegistroDeLocacaoAsync(int index, int livroId)
        {
            var registroDeLocacao = new RegistroDeLocacaoVM
            {
                Index = index,
                Id = index * -1,
                LivroId = livroId,
                Livro = await _repository.GetByIdAsync(livroId),
                DataDeSaida = DateTime.Now                
            };

            return Partial("_RegistrosDeLocacaoGridRow", registroDeLocacao);
        }

        private async Task<LocacaoVM> FiltrarRegistrosAsync(int livroId, DateTime? dataDeSaidaInicial, DateTime? dataDeDevolucaoInicial)
        {
            var locacao = new LocacaoVM()
            {
                LivroId = livroId,
                DataDeSaidaInicial = dataDeSaidaInicial,
                DataDeDevolucaoInicial = dataDeDevolucaoInicial,
            };

            var livro = await _repository.GetByIdAsync(livroId);

            //Recuperar os Regsitros de Locação (no caso vou simular...)

            if (livro != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    var dataDeSaida = DateTime.Now.AddDays(i * -1).AddHours(i * -1);

                    locacao.Registros.Add(new RegistroDeLocacaoVM()
                    {
                        Id = (i + 1),
                        Livro = livro,
                        LivroId = livro.Id,
                        DataDeSaida = dataDeSaida,
                        DataDeDevolucao = (i % 2 == 0) ? null : dataDeSaida.AddDays(i).AddHours(i),
                        Socio = $"Sócio # {(i + 1)}"
                    });
                }
            }

            return locacao;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Locacao = new LocacaoVM();

            await BindSelectListsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await BindSelectListsAsync();
                return Page();
            }

            if (!Locacao.Registros.Any())
            {
                ModelState.AddModelError("Registros", "Não é Possível Grvar pois não Foram Informados Registros de Locação.");
                await BindSelectListsAsync();
                return Page();
            }

            try
            {
                //Gravar a Locação aqui
            }
            catch (KeyNotFoundException)
            {
                await BindSelectListsAsync();
                return NotFound();
            }
            catch (ValidationListException ex)
            {
                await BindSelectListsAsync();

                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.Key, error.Value);

                return Page();
            }

            return RedirectToPage("./Locacao");
        }
    }

    public class LocacaoVM
    {
        [Required]
        [Display(Name = "Livro")]
        public int LivroId { get; set; } = 0;

        [Display(Name = "Saida Inicial")]
        public DateTime? DataDeSaidaInicial { get; set; } = null;

        [Display(Name = "Devolução Inicial")]
        public DateTime? DataDeDevolucaoInicial { get; set; } = null;

        public List<RegistroDeLocacaoVM> Registros { get; set; } = new List<RegistroDeLocacaoVM>();
    }

    public class RegistroDeLocacaoVM : IPermiteMarcarParaExclusao
    {
        public int Index { get; set; } = 0;

        [Display(Name = "#")]
        public int Id { get; set; }

        [Display(Name = "Livro")]
        public Livro? Livro { get; set; }

        [Required]
        [Display(Name = "Livro")]
        public int LivroId { get; set; }

        [Required]
        [Display(Name = "Data Saída")]
        public DateTime DataDeSaida { get; set; } = DateTime.Now;

        [Display(Name = "Data Devolução")]
        public DateTime? DataDeDevolucao { get; set; }

        [Required]
        [Display(Name = "Sócio")]
        public string Socio { get; set; } = string.Empty;

        public bool MarcadoParaExclusao { get; set; } = false;
    }
}
