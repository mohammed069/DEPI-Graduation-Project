using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MissingPersons.DAL.Models;
using MissingPersons.MVC;
namespace MissingPersons.MVC;

public static class DatabaseMigrator
{
    public static async Task MigratorAndSeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<LostFoundContext>();
        await context.Database.MigrateAsync();

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        string[] roles = { AppRole.Admin, AppRole.User };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }

        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        const string adminEmail = "admin@lostfound.com";
        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Admin",
                EmailConfirmed = true,
                CreatedAt = DateTime.Now
            };

            var result = await userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, AppRole.Admin);
            }
        }
    }
}

