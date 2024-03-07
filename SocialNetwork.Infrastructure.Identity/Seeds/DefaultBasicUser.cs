using Microsoft.AspNetCore.Identity;
using SocialNetwork.Infrastructure.Identity.Enums;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Infrastructure.Identity.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultUser = new();
            defaultUser.UserName = "basicuser";
            defaultUser.Email = "basicuser@email.com";
            defaultUser.Name = "Heung Min";
            defaultUser.LastName = "Son";
            defaultUser.ProfilePicture = "https://leadership.civicaction.ca/wp-content/uploads/2016/11/Foundation-Website-Mark-Sam.png";
            defaultUser.PhoneNumber = "(123) 456-7890";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;

            if(userManager.Users.All(u=> u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }
            }
         
        }
    }
}
