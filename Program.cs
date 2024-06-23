using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Contracts;
using ProductManagement.Data;
using ProductManagement.Services;
using ProductManagement.Controllers;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ProductManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ProductDbContext>();
            builder.Services.AddControllersWithViews();
            //builder.Services.AddScoped<ILocationApiService, LocationApiService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

                        builder.Services.AddEndpointsApiExplorer();

                        builder.Services.AddSwaggerGen();


            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger"; // Serve Swagger UI at the app's root
            });

            
            //app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();


            app.Run();
        }
    }
}