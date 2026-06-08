using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsMVC .Data;
using MedicMoveLogisticsMVC.Services;

namespace MedicMoveLogisticsMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC
            builder.Services.AddControllersWithViews();
            
            builder.Services.AddHttpClient<ClientService>();
            builder.Services.AddHttpClient<ContractService>();
            builder.Services.AddHttpClient<AuthService>();
            builder.Services.AddHttpClient<ServiceRequestsService>();

            // API Calls
            builder.Services.AddHttpClient();

            // Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
                    ?? builder.Configuration.GetConnectionString("DefaultConnection")));

            // Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Create database if it doesn't exist
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Database creation failed.");
                }
            }

            // Configure pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}