using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Riode.WebUI.Models.DataContext;
using System.IO;

namespace Riode.WebUI
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddRouting(cfg => cfg.LowercaseUrls=true);

            services.AddDbContext<RiodeDBContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapGet("/comingsSoon.html", async(context)=>{
                   using (var sr = new StreamReader("views/Static/comingsSoon.html"))
                    {
                        context.Response.ContentType = "text/html";
                       await context.Response.WriteAsync(sr.ReadToEnd());
                    }
                }
                    );
                endpoints.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
