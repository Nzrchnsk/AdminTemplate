using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Core.Constants;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(
                Enum.GetName(typeof(Core.Constants.ApplicationRoles), Core.Constants.ApplicationRoles.Administrator))
            );

            var defaultUser = new ApplicationUser(AuthorizationConstants.DEFAULT_USER_USERNAME);
            await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);

            var adminUser = new ApplicationUser(AuthorizationConstants.DEFAULT_ADMIN_USERNAME);
            await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
            adminUser = await userManager.FindByNameAsync(AuthorizationConstants.DEFAULT_ADMIN_USERNAME);
            await userManager.AddToRoleAsync(adminUser,
                Enum.GetName(typeof(Core.Constants.ApplicationRoles), Core.Constants.ApplicationRoles.Administrator));
        }
    }
}