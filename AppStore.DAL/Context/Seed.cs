using AppStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppStore.DAL.Context
{
    public static class Seed
    {
        public static async Task Initialize(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            if (!await context.Roles.AnyAsync())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

                await roleManager.CreateAsync(new IdentityRole<int>(UserRole.USER));

                await roleManager.CreateAsync(new IdentityRole<int>(UserRole.DEVELOPER));

                await roleManager.CreateAsync(new IdentityRole<int>(UserRole.ADMIN));
            }

            if (!await context.Users.AnyAsync())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var adminRole = context.Roles.FirstOrDefault(r => r.Name == UserRole.ADMIN);

                if (adminRole != null)
                {
                    var adminUser = new User
                    {
                        UserName = "admin",
                        Email = "admin@gmail.com",
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var createResult = await userManager.CreateAsync(adminUser, "Admin123!");

                    if (createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, UserRole.ADMIN);
                    }
                }
            }
        }
    }
}
