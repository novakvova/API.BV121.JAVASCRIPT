using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShop.Abastract;
using WebShop.Constants;
using WebShop.Data.Entities.Identity;
using WebShop.Models;

namespace WebShop.Data
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope =
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<ISmtpEmailService>();
                context.Database.Migrate();
                var userManager = scope.ServiceProvider
                    .GetRequiredService<UserManager<UserEntity>>();

                var roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<RoleEntity>>();

                if (!context.Roles.Any())
                {
                    RoleEntity admin = new RoleEntity
                    {
                        Name = Roles.Admin,
                    };
                    RoleEntity user = new RoleEntity
                    {
                        Name = Roles.User,
                    };
                    var result = roleManager.CreateAsync(admin).Result;
                    result = roleManager.CreateAsync(user).Result;
                }

                if (!context.Users.Any())
                {
                    UserEntity user = new UserEntity
                    {
                        FirstName = "Марко",
                        LastName = "Муха",
                        Email = "muxa@gmail.com",
                        UserName = "muxa@gmail.com",
                    };
                    var result = userManager.CreateAsync(user, "123456")
                        .Result;
                    if (result.Succeeded)
                    {
                        result = userManager
                            .AddToRoleAsync(user, Roles.Admin)
                            .Result;
                    }
                }

                //Message message = new Message();
                //message.Subject = "Це прикольний шаблон";
                //var dir = Path.Combine(Directory.GetCurrentDirectory(), "email-template/index.html");
                //string html = File.ReadAllText(dir);
                //message.Body = html; //"Беріть гернератори, інвертери або свічки і усе буде круто";
                //message.To = "novakvova@gmail.com";
                //emailService.Send(message);
            }
        }
    }
}
