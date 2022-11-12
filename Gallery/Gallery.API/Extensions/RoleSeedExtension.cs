using Gallery.DAL;
using Gallery.DAL.Domain;
using Microsoft.AspNetCore.Identity;

namespace Gallery.API.Extensions
{
    /// <summary>
    /// Manage default users and roles of the application
    /// </summary>
    public static class RoleSeedExtension
    {
        private class UserRoleHelper
        {
            public ApplicationUser UserDTO { get; set; }
            public string Role { get; set; } = string.Empty;
        }

        /// <summary>
        /// Add roles and users from settings
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public async static void AddRoleSeedExtensionExtensions(this IServiceProvider services, IConfiguration configuration)
        {
            using (var scope = services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var roles = configuration.GetSection("Roles").Get<List<ApplicationRole>>();
                foreach (var role in roles)
                {
                    var alreadyExists = await roleManager.RoleExistsAsync(role.Name);
                    if (!alreadyExists)
                    {
                        await roleManager.CreateAsync(role);
                    }
                }
                var userRoles = configuration.GetSection("DefaultUsers").Get<List<UserRoleHelper>>();
                var secretPW = configuration.GetValue<string>("AdminPass");
                foreach (var userRole in userRoles)
                {
                    var alreadyExists = (await userManager.FindByNameAsync(userRole.UserDTO.UserName)) != null;
                    if (!alreadyExists)
                    {
                        IdentityResult checkAdd = await userManager.CreateAsync(userRole.UserDTO, secretPW);
                        if (checkAdd.Succeeded)
                        {
                            await userManager.AddToRoleAsync(userRole.UserDTO, userRole.Role);
                        }
                    }
                }
            }
        }
    }
}
