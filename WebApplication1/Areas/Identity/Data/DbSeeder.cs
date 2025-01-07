using Microsoft.AspNetCore.Identity;
using ShopMVC2.Constants;
using WebApplication1.Areas.Identity.Data;

namespace ShopMVC2.Areas.Identity.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            var admin = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@gmail.com"
            };

            var userInDb = await userManager.FindByEmailAsync(admin.Email);

            if (userInDb == null)
            {
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }
    }
}
