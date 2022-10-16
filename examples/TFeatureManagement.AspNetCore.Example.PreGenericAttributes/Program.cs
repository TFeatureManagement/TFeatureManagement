using TFeatureManagement.AspNetCore.DependencyInjection;
using TFeatureManagement.AspNetCore.Example.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddFeatureManagement<Feature>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "featureconstraint",
    pattern: "Home/FeatureConstrained/{id?}",
    defaults: new { controller = "Home", action = "FeatureConstrained" });

app.MapControllerRoute(
    name: "featureconstraintfallback",
    pattern: "Home/FeatureConstrained/{id?}",
    defaults: new { controller = "Home", action = "FeatureConstrainedFallback" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
