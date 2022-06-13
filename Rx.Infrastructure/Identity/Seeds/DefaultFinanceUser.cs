using Microsoft.AspNetCore.Identity;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Enums;

namespace Rx.Infrastructure.Identity.Seeds;

public static class DefaultFinanceUser
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Default User
        var defaultFUser = new ApplicationUser
        {
            UserName = "financeUser",
            Email = "fuser@gmail.com",
            FullName = "Finance User",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultFUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultFUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultFUser, "123Pa$$word!");
                await userManager.AddToRoleAsync(defaultFUser, Roles.FinanceUser.ToString());
            }

        }
    }
}