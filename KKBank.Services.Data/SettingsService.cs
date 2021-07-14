using KKBank.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace KKBank.Services.Data
{
    public class SettingsService : ISettingsService
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public SettingsService(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task CreateRole(string roleName)
        {
            bool x = await roleManager.RoleExistsAsync(roleName);
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = roleName;
                await roleManager.CreateAsync(role);
            }
        }
    }
}
