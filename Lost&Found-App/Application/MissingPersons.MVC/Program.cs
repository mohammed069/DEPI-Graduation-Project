using Microsoft.EntityFrameworkCore;
using MissingPersons.BLL;
using MissingPersons.DAL.Models;
using Microsoft.AspNetCore.Identity;
using MissingPersons.MVC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
   // ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<LostFoundContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User,IdentityRole<int>>().AddEntityFrameworkStores<LostFoundContext>().AddDefaultTokenProviders();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
using (var scope=app.Services.CreateScope())
{
    await DatabaseMigrator.MigratorAndSeedAsync(scope.ServiceProvider);
}
if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();