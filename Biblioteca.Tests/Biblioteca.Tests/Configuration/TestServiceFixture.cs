using Biblioteca.WebApp.Data;
using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Abstractions.Services;
using Biblioteca.WebApp.Infrastructure.Repositories;
using Biblioteca.WebApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteca.Tests.Configuration
{
    public class TestServiceFixture
    {
        public ServiceProvider ServiceProvider { get; }

        public TestServiceFixture()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Serviços
            services.AddTransient<IReportService, ReportService>();

            // Repositórios
            services.AddScoped<IAssuntoRepository, AssuntoRepository>();
            services.AddScoped<IAutorRepository, AutorRepository>();
            services.AddScoped<ILivroRepository, LivroRepository>();
            services.AddScoped<IPrecoDeVendaRepository, PrecoDeVendaRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
