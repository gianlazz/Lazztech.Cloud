using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Data
{
    public class DbSeeder : IDbSeeder
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public DbSeeder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public void SeedAdminUser()
        {
            var defaultAdminSection = _configuration.GetSection("DefaultAdminUser");
            var email = defaultAdminSection["Email"];
            var password = defaultAdminSection["Password"];

            if (_userManager.FindByEmailAsync(email).Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = email,
                    Email = email
                };

                IdentityResult identitySuceeded = _userManager.CreateAsync(user, password).Result;

                if (identitySuceeded.Succeeded)
                {
                    var roleName = "Admin";
                    var roleSucceded = _roleManager.CreateAsync(new IdentityRole(roleName)).Result;
                    _userManager.AddToRoleAsync(user, roleName).Wait();
                }
            }
        }
    }
}
