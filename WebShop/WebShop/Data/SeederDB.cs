using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using System.Globalization;
using WebShop.Abastract;
using WebShop.Constants;
using WebShop.Data.Entities;
using WebShop.Data.Entities.Identity;
using WebShop.Helpers;
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

                if (!context.Categories.Any())
                {
                    var cultureInfo = new CultureInfo("uk-UA");

                    CategoryEntity cat = new CategoryEntity()
                    {
                        Name = "Pizza",
                        DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        Image = "pizza.jpg"
                    };
                    context.Categories.Add(cat);
                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    var testProduct = new Faker<ProductEntity>()
                        .RuleFor(u => u.Name, (f, u) => f.Commerce.Product())
                        .RuleFor(u => u.Price, (f, u) => decimal.Parse(f.Commerce.Price()))
                        .RuleFor(u => u.DateCreated, (f, u) => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
                        .RuleFor(u => u.Description, (f, u) => f.Commerce.ProductDescription())
                        .RuleFor(u => u.CategoryId, (f, u) => 1);
                    for (int i = 0; i < 20; i++)
                    {
                        var p = testProduct.Generate();
                        context.Products.Add(p);
                        context.SaveChanges();

                        var testProductImage = new Faker<ProductImageEntity>()
                       .RuleFor(u => u.DateCreated, (f, u) => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
                       .RuleFor(u => u.ProductId, (f, u) => p.Id)
                       .RuleFor(u => u.Name, f => f.Image.LoremFlickrUrl());

                        for (int j = 0; j < 3; j++)
                        {
                            var img = testProductImage.Generate();
                            string name = AddImage(app, img.Name);
                            img.Name = name;
                            context.ProductImages.Add(img);
                            context.SaveChanges();
                        }
                    }

                }
            }
        }

        private static string AddImage(IApplicationBuilder app, string urlImage)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                string fileName = String.Empty;

                if (urlImage != null)
                {
                    var bmp = ImageWorker.UrlToBitmap(urlImage);

                    fileName = Path.GetRandomFileName() + ".jpg";
                    string[] imageSizes = ((string)_configuration.GetValue<string>("ImageSizes")).Split(" ");
                    foreach (var imageSize in imageSizes)
                    {
                        int size = int.Parse(imageSize);
                        string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", $"{size}_{fileName}");

                        var saveImage = ImageWorker.CompressImage(bmp, size, size, false, false);
                        saveImage.Save(dirSaveImage, ImageFormat.Jpeg);
                    }
                    return fileName;

                }
            }
            return null;
        }
    }
}
