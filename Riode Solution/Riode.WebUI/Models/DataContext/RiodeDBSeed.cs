using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Riode.WebUI.Models.Entities.Membership;

namespace Riode.WebUI.Models.DataContext
{
    public static class RiodeDBSeed
    {
        static public IApplicationBuilder SeedMembership(this IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {

                var role = new RiodeRole()
                {
                    Name = "SuperAdmin"
                };


                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<RiodeUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RiodeRole>>();

                var hasRole = roleManager.RoleExistsAsync(role.Name).Result;
                if (hasRole == true)
                {
                    var roleResult = roleManager.FindByNameAsync(role.Name).Result;
                }
                else
                {
                    var iResult = roleManager.CreateAsync(role).Result;
                    if (!iResult.Succeeded)
                    {
                        goto end;
                    }
                }

                var user = new RiodeUser()
                {
                    UserName = "Ilham",
                    Email = "ilhambagirov027@gmail.com",
                    EmailConfirmed = true
                };

                var foundUser = userManager.FindByEmailAsync(user.Email).Result;

                if (foundUser != null && !userManager.IsInRoleAsync(foundUser, role.Name).Result)
                {
                    userManager.AddToRoleAsync(foundUser, role.Name).Wait();
                }
                else if (foundUser == null)
                {
                    string password = "123";
                    var iUserResult = userManager.CreateAsync(user, password).Result;
                    if (iUserResult.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, role.Name).Wait();
                    }
                }

            }

        end:
            return app;
        }
    }
}
