using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsAPI.Data;
namespace MedicMoveLogisticsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
 // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")));


            builder.Services.AddControllers();
            builder.Services.AddOpenApi(); // Keeps the native engine
            var app = builder.Build();
            if (true)
            {
                app.MapOpenApi(); // Generates /openapi/v1.json

                app.UseSwaggerUI(options =>
                {
                    // Tell Swagger UI to look at the native .NET OpenAPI endpoint
                    options.SwaggerEndpoint("/openapi/v1.json", "Medic Move Logistics API v1");
                    options.RoutePrefix = "swagger";
                });
            }


            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
