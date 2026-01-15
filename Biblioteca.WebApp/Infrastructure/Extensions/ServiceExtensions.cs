using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Biblioteca.WebApp.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureCulture(this WebApplication app)
        {
            var cultureInfo = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var supportedCultures = new[] { "pt-BR", "pt-PT" };

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = new[] { new CultureInfo("pt-BR") },
                SupportedUICultures = new[] { new CultureInfo("pt-BR") }
            };

            app.UseRequestLocalization(localizationOptions);

            return app;
        }
    }
}
