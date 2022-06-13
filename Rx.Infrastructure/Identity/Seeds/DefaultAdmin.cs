using Microsoft.AspNetCore.Identity;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Enums;

namespace Rx.Infrastructure.Identity.Seeds;

public static class DefaultAdmin
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Default User
        var defaultAdmin = new ApplicationUser
        {
            UserName = "accountAdmin",
            Email = "admin@gmail.com",
            FullName = "Akshayan Thiru",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultAdmin.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultAdmin.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultAdmin, "123Pa$$word!");
                await userManager.AddToRolesAsync(defaultAdmin, new[] { Roles.FinanceUser.ToString(), Roles.Admin.ToString() });
            }

        }
    }
}