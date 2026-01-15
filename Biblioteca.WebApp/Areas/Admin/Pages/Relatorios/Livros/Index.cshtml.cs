using Biblioteca.WebApp.Data;
using Biblioteca.WebApp.Model;
using Biblioteca.WebApp.Model.Views;
using Biblioteca.WebApp.PageModels;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Areas.Admin.Pages.Relatorios.Livros
{
    public class IndexModel : PagerAndFilterPageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<ReportLivrosGroupView> ReportLivrosViewList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (!Request.QueryString.HasValue)
            {
                ReportLivrosViewList = new List<ReportLivrosGroupView>();
                return;
            }

            int pageSize = 1000;

            var query = _dbContext
                            .ReportLivrosViewSet
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var filtros = Search.Split(';');

                if (filtros.Count() == 1 && filtros[0].Split(':').Count() == 1)
                {
                    query = query.Where(w =>
                        w.Autor.Contains(Search));
                }
                else
                {
                    var campoValorList = Search.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(f => f.Split(':', 2))
                                        .Where(a => a.Length == 2)
                                        .Select(a => new { Campo = a[0].Trim(), Valor = a[1].Trim() });

                    foreach (var f in campoValorList)
                    {
                        switch (f.Campo.ToLower())
                        {
                            case "livro":
                                query = query.Where(w => w.Livro != null && w.Livro.Contains(f.Valor));
                                break;
                            case "autor":
                                query = query.Where(w => w.Autor != null && w.Autor.Contains(f.Valor));
                                break;
                            case "assunto":
                                query = query.Where(w => w.Assunto != null && w.Assunto.Contains(f.Valor));
                                break;
                        }
                    }
                }
            }

            int total = await query.CountAsync();

            TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            var viewResultSet = await query
                .Skip((PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ReportLivrosViewList =
                viewResultSet
                .GroupBy(x => x.Autor)
                .Select(x => new ReportLivrosGroupView()
                {
                    Autor = x.Key,
                    Livros = x.GroupBy(y => y.Livro)
                                .Select(y => new LivroGroupView()
                                {
                                    Livro = y.Key,
                                    Assunto = String.Join(",", y.Select(z => z.Assunto))
                                })
                                .OrderBy(y => y.Livro)
                                .ToList()
                })
                .OrderBy(y => y.Autor)
                .ToList();
        }
    }
}
