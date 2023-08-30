 using WebApp.TemplatePattern.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp.TemplatePattern
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppIdentityDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();












            var app = builder.Build();


            using var scope=app.Services.CreateScope();
                var identityDbContext=scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                identityDbContext.Database.Migrate();
            
            if(!userManager.Users.Any())
            {
                userManager.CreateAsync(new AppUser { Email = "user1@outlook.com", UserName = "user1",PictureUrl = "/userpictures/primeuserpicture.png",Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book." }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser { Email = "user2@outlook.com", UserName = "user2", PictureUrl = "/userpictures/primeuserpicture.png", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book." }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser { Email = "user3@outlook.com", UserName = "user3", PictureUrl = "/userpictures/primeuserpicture.png", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book." }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser {Email = "user4@outlook.com", UserName = "user4", PictureUrl = "/userpictures/primeuserpicture.png", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book." }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser {Email = "user5@outlook.com", UserName = "user5", PictureUrl = "/userpictures/primeuserpicture.png", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book." }, "Password12*").Wait();
            }
            



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}