using Microsoft.EntityFrameworkCore;
using prjHealthCareSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//---------------------------------------


//Add services to the container.
    builder.Services.AddControllersWithViews();
    

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ));

  //1. Add session services to the dependency injection containor
  builder.Services.AddSession(options =>
  {
      options.IdleTimeout = TimeSpan.FromMinutes(20); //The session will expire
                                                      //after 20 minutes of inactivity
      options.Cookie.HttpOnly = true;//Makes the cookie inaccessible to client-side script
      options.Cookie.IsEssential = true;//Required for GDPR compliance
  });
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        //Give SQL server 45 seconds to warm up in Doncker
      
        logger.LogInformation("Waiting 10 seconds for Server to boot...");
    
    context.Database.EnsureCreated();
    logger.LogInformation("Database and tables created successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
 // 2.IMPORTANT: Add the sesssion middleware BEFORE UseAuthorized and MapControllerRoute
 app.UseSession();
//PDF file access
app.UseStaticFiles();       

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
