using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Model;
using Biblioteca.WebApp.PageModels;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace Biblioteca.WebApp.Areas.Admin.Pages.General.Autores
{
    public class IndexModel : PagerAndFilterPageModel
    {
        protected readonly IAutorRepository _repository;
        public IndexModel(IAutorRepository repository)
        {
            _repository = repository;
        }

        public IList<Autor> Autores { get; set; } = default!;

        public async Task OnGetAsync()
        {
            int pageSize = 10;

            var query = _repository.Query()
                .OrderBy(w => w.Nome)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var filtros = Search.Split(';');

                if (filtros.Count() == 1 && filtros[0].Split(':').Count() == 1)
                {
                    query = query.Where(w =>
                        w.Nome.Contains(Search));
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
                            case "nome":
                                query = query.Where(w => w.Nome.Contains(f.Valor));
                                break;
                        }
                    }
                }
            }

            int total = await query.CountAsync();

            TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            Autores = await query
                .Skip((PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
