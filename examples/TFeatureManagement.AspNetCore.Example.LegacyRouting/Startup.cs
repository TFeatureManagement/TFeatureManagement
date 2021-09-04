using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TFeatureManagement.AspNetCore.DependencyInjection;
using TFeatureManagement.AspNetCore.Example.Models;

namespace TFeatureManagement.AspNetCore.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews((options) => options.EnableEndpointRouting = false);

            services.AddFeatureManagement<Feature>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see
                // https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Disable adding the endpoint routing middleware.
            //app.UseRouting();

            app.UseAuthorization();

            // Disable adding endpoints.
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

            // Add routes using the ASP.NET Core 2.1 routing based on IRouter.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "featureconstraint",
                    template: "Home/FeatureConstrained/{id?}",
                    defaults: new { controller = "Home", action = "FeatureConstrained" });

                routes.MapRoute(
                    name: "featureconstraintfallback",
                    template: "Home/FeatureConstrained/{id?}",
                    defaults: new { controller = "Home", action = "FeatureConstrainedFallback" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}