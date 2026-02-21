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

                    var usuarios = new[]
                    {
                        new IdentityUser
                        {
                            UserName = "anderson.neto.silva@hotmail.com",
                            Email = "anderson.neto.silva@hotmail.com",
                            EmailConfirmed = true
                        },
                        new IdentityUser
                        {
                            UserName = "gviegas@hotmail.com",
                            Email = "gviegas@hotmail.com",
                            EmailConfirmed = true
                        }
                    };


                    foreach (var usuario in usuarios)
                    {
                        var oldUser = await userManager.FindByEmailAsync(usuario.UserName);

                        if (oldUser == null)
                        {
                            var result = await userManager.CreateAsync(usuario, "PasswordSegura123!");

                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(usuario, "Admin");
                            }
                        }
                        else
                            await userManager.AddToRoleAsync(oldUser, "Admin");
                    }

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