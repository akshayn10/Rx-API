using Microsoft.AspNetCore.Identity;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Enums;

namespace Rx.Infrastructure.Identity.Seeds;

public static class DefaultRoles
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole(Roles.Owner.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.FinanceUser.ToString()));
    } 
}