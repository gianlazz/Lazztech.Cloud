using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.ObsidianPresences.ClientFacade
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, string userName, string email, string password)
        {
            if (userManager.FindByEmailAsync(email).Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = userName,
                    Email = email
                };

                IdentityResult identitySuceeded = userManager.CreateAsync(user, password).Result;

                if (identitySuceeded.Succeeded)
                {
                    var roleName = "Admin";
                    var roleSucceded = roleManager.CreateAsync(new IdentityRole(roleName)).Result;
                    userManager.AddToRoleAsync(user, roleName).Wait();
                }
            }
        }
    }
}
