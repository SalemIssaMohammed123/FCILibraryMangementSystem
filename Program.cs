using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Test.Repositories.Publisher;
using Test.Repositories.Author;
using Test.Repositories.Departement;
using Test.Repositories.Book;
using Test.Models;
using Microsoft.AspNetCore.Identity;
namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //built in services and not register in the IOC container
            //Register the db context
            builder.Services.AddDbContext<Test.Models.Context>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("Db_Connection"));
            });
            //built in services for identity and not register in the IOC container
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<Test.Models.Context>().AddDefaultTokenProviders(); // Register the default token provider;
            //custom services that need to create and register in the IOC container
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
            builder.Services.AddScoped<IDepartementRepository, DepartementRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); //Filter "Authorise" can authorise cookie
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}