using IdentityApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Data
{
    public static class AppSeedData
    {
        private const string adminUser = "adminUser";
        private const string adminPassword = "abc123";
        private const string adminFullName = "Admin User";
        private const string adminEmailAddress = "admin@email.com";
        private const string adminPhoneNumber = "123456789";

        public static async Task IdentityTestUser(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            var adminRole = await roleManager.FindByNameAsync("ADMIN");
            if (adminRole == null)
            {
                await roleManager.CreateAsync(new AppRole { Name = "ADMIN" });
            }

            var user = await userManager.FindByNameAsync(adminUser);

            if (user == null)
            {
                user = new AppUser
                {
                    FullName = adminFullName,
                    UserName = adminUser,
                    Email = adminEmailAddress,
                    PhoneNumber = adminPhoneNumber,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, adminPassword);
                await userManager.AddToRoleAsync(user, "ADMIN");
            }
        }
    }
}
