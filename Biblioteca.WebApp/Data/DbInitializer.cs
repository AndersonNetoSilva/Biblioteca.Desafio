namespace Biblioteca.WebApp.Data
{
    public static class DbInitializer
    {
        public static async Task SeddAsync(IServiceProvider rootServiceProvider)
        {
            using (var scope = rootServiceProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                try
                {

                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ocorreu um erro ao popular a base de dados (Data Seeding).");
                }
            }

        }
    }
}
