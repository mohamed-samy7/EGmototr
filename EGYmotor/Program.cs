using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EGYmotor.Data;
namespace EGYmotor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<EGYmotorContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EGYmotorContext") ?? throw new InvalidOperationException("Connection string 'EGYmotorContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();/////////////////
            builder.Services.AddHttpContextAccessor();/////////////


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

            app.UseSession();////////////

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
