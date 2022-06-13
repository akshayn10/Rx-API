using Microsoft.AspNetCore.Identity;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Enums;

namespace Rx.Infrastructure.Identity.Seeds;

public static class DefaultOwner
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Default User
        var defaultOwner = new ApplicationUser
        {
            UserName = "accountOwner",
            Email = "owner@gmail.com",
            FullName = "Account Owner",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultOwner.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultOwner.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultOwner, "123Pa$$word!");
                await userManager.AddToRolesAsync(defaultOwner,new []{Roles.Admin.ToString(),Roles.FinanceUser.ToString(),Roles.Owner.ToString()});
            }
        }
    }
}