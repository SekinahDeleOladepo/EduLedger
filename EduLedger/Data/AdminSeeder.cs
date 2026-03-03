using EduLedger.Entitites.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;

namespace EduLedger.Data
{
    public static class AdminSeeder
    {
        private const string AdminEmail = "admin@eduledger.com";
        private const string AdminPassword = "Admin@123";
        private const string AdminRole = "Admin";

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var existingAdmin = await userManager.FindByEmailAsync(AdminEmail);

            if (existingAdmin != null)
                return;

            var admin = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                EmailConfirmed = true,
               
            };

            var result = await userManager.CreateAsync(admin, AdminPassword);

            if (!result.Succeeded)
                return;

            await userManager.AddToRoleAsync(admin, AdminRole);
        }
    }
}
