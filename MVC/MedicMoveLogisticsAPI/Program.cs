using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsAPI.Data;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace MedicMoveLogisticsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
//Chandra, S 2026.
            // Add services to the container.
 // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")));
        
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "MedicMoveLogisticsIssuer",
                        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "MedicMoveLogisticsAudience",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "ae1ff2e981a483faf7b29c6fa5f40d4c2e2cd877f76d0530afd3847b5988e585"))
                    };
                    
                });

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
    {
        // This tells the serializer to ignore loops instead of crashing
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    builder.Services.AddHttpClient();
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
/*
Chandra, S. 2026. JWT Authentication in ASP.NET Core: A complete step-bystey guide. (Version 10.0) [Source Code]. Available at: <https://medium.com/@Rajdip27/jwt-authentication-in-asp-net-core-a-complete-step-by-step-guide-94c955d907da> [Accessed 7 June 2026]
*/