using WebApp.StrategyPattern.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.StrategyPattern.Repositories;
using System.Runtime.InteropServices;

namespace WebApp.StrategyPattern
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

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IProductRepository>(sp =>
            {
                var httpContext = sp.GetRequiredService<IHttpContextAccessor>();
                var claim = httpContext.HttpContext.User.Claims.Where(x => x.Type == Settings.ClaimdatabaseType).FirstOrDefault();
                var context = sp.GetRequiredService<AppIdentityDbContext>();
                if (claim == null) return new ProductRepositoryFromSqlServer(context);

                var databaseType = (EDatabaseType)int.Parse(claim.Value);
                return databaseType switch
                {
                    EDatabaseType.SqlServer => new ProductRepositoryFromSqlServer(context),
                    EDatabaseType.MongoDb => new ProductRepositoryFromMongoDb(builder.Configuration),
                    _ => throw new NotImplementedException()


                };


            });










            var app = builder.Build();


            using var scope = app.Services.CreateScope();
            var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            identityDbContext.Database.Migrate();

            if (!userManager.Users.Any())
            {
                userManager.CreateAsync(new AppUser { Email = "user1@outlook.com", UserName = "user1" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser { Email = "user2@outlook.com", UserName = "user2" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser { Email = "user3@outlook.com", UserName = "user3" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser { Email = "user4@outlook.com", UserName = "user4" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser { Email = "user5@outlook.com", UserName = "user5" }, "Password12*").Wait();
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