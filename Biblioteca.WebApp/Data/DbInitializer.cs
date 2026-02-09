using Microsoft.AspNetCore.Identity;

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
        public static async Task SeedRolesAndAdminAsync(IServiceProvider rootServiceProvider)
        {
            using (var scope = rootServiceProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        var role = new IdentityRole("Admin");

                        var result = await roleManager.CreateAsync(role);

                        if (result.Succeeded)
                        {
                            await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("system_main_claim", "Admin"));
                        }
                    }

                    if (!await roleManager.RoleExistsAsync("User"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    var adminUser = await userManager.FindByEmailAsync("anderson.neto.silva@hotmail.com");

                    if (adminUser == null)
                    {
                        var user = new IdentityUser
                        {
                            UserName = "anderson.neto.silva@hotmail.com",
                            Email = "anderson.neto.silva@hotmail.com",
                            EmailConfirmed = true
                        };

                        var result = await userManager.CreateAsync(user, "PasswordSegura123!");

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                    }
                    else
                        await userManager.AddToRoleAsync(adminUser, "Admin");
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