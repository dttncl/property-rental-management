using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;
using System.Runtime.Intrinsics.Arm;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("RentaSpaceDatabase");
builder.Services.AddDbContext <property_rental_management.Models.RentaSpaceDbContext> (options => options.UseSqlServer(connection));

builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
 name: "default",
 pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();