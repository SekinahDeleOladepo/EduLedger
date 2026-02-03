using Microsoft.AspNetCore.Identity;

namespace EduLedger.Data
{
    public class AdminSeeder
    {
        public static async Task SeedAdminAsync(
       UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@eduledger.com";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
