using Biblioteca.WebApp.Data;
using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Abstractions.Services;
using Biblioteca.WebApp.Infrastructure.Repositories;
using Biblioteca.WebApp.Infrastructure.Services;

namespace Biblioteca.WebApp.Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Serviços
            services.AddTransient<IReportService, ReportService>();

            // Repositórios
            services.AddScoped<IAssuntoRepository, AssuntoRepository>();
            services.AddScoped<IAutorRepository, AutorRepository>();
            services.AddScoped<ILivroRepository, LivroRepository>();
            services.AddScoped<IPrecoDeVendaRepository, PrecoDeVendaRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();

            return services;
        }
    }
}
