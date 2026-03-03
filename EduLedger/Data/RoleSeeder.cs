using Microsoft.AspNetCore.Identity;

namespace EduLedger.Data
{
    public static class RoleSeeder
    {
        private static readonly string[] Roles =
        {
            "Admin",
            "Instructor",
            "Student"
        };

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
