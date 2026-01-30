using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILivroRepository _livroRepository;

        public IndexModel(ILogger<IndexModel> logger, ILivroRepository livroRepository)
        {
            _logger = logger;
            _livroRepository = livroRepository;
        }


        public List<AutoresPorLivro> AutoresPorLivroList { get; set; }
        public List<DonutsModel> DonutsModeList { get; set; }

        public Notificacoes Notificacoes { get; set; } 

        public void OnGet()
        {
            AutoresPorLivroList = GetAutoresPorLivroList();
            DonutsModeList = GetDonutsModelList();
            Notificacoes = GetNotificacoes();
        }

        private Notificacoes GetNotificacoes()
        {
            return new Notificacoes()
            {
                AniversariantesDoMes = "Joselito 15/04, Antonio 20/02"
            };
        }

        private List<DonutsModel> GetDonutsModelList()
        {
            return _livroRepository
                .Query()
                .Include(x => x.Autores)
                .GroupBy(x => x.Titulo)
                .Select(x => new DonutsModel(x.Select(y => y.Autores.Count()).First(), x.Key))
                .ToList();
        }

        private List<AutoresPorLivro> GetAutoresPorLivroList()
        {
            return _livroRepository
                .Query()
                .Include(x => x.Autores)
                .GroupBy(x => x.Titulo)
                .Select(x => new AutoresPorLivro(x.Select(y => y.Autores.Count()).First(), x.Key))
                .ToList();
        }
    }

    [Serializable]
    public class AutoresPorLivro
    {
        public AutoresPorLivro()
        {

        }
        public AutoresPorLivro(int quantidade, string livro)
        {
            Quantidade = quantidade;
            Livro = livro;
        }

        public int Quantidade { get; set; }

        public string Livro { get; set; }
    }

    [Serializable]
    public class Notificacoes
    {
        public string AniversariantesDoMes { get; set; }
    }

    [Serializable]
    public class DonutsModel
    {
        public DonutsModel()
        {

        }
        public DonutsModel(int value, string label)
        {
            Value = value;
            Label = label;
        }

        public int Value { get; set; }

        public string Label { get; set; }
    }
}
