 using DinkToPdf;
 using DinkToPdf.Contracts;
 using WebApp.CommandPattern.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
 using WebApp.CommandPattern.Commands;

 namespace WebApp.CommandPattern
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
            builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            builder.Services.AddScoped(typeof(IPdfFile<>), typeof(PdfFile<>));
            builder.Services.AddScoped(typeof(IExcelFile<>), typeof(ExcelFile<>));





             







            var app = builder.Build();


            using var scope=app.Services.CreateScope();
                var identityDbContext=scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                identityDbContext.Database.Migrate();
            
            if(!userManager.Users.Any())
            {
                userManager.CreateAsync(new AppUser { Email = "user1@outlook.com", UserName = "user1" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser { Email = "user2@outlook.com", UserName = "user2" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser { Email = "user3@outlook.com", UserName = "user3" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser { Email = "user4@outlook.com", UserName = "user4" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser { Email = "user5@outlook.com", UserName = "user5" }, "Password12*").Wait();


                Enumerable.Range(1,30).ToList().ForEach(x =>
                {
                    identityDbContext.Products.Add(new Product()
                    {
                        Name = $"kalem {x}", Price = x * 100, Stock = x * 50
                    });
                    identityDbContext.SaveChanges();
                });
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