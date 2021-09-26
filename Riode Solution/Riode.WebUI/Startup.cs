using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Riode.WebUI.AppCode.Middlewares;
using Riode.WebUI.AppCode.Provider;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities.Membership;
using System.IO;
using System.Reflection;

namespace Riode.WebUI
{
    public class Startup
    {


        IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;


        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(cfg =>
            {
                cfg.ModelBinderProviders.Insert(0, new BooleanBinderProvider());

                /*var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));*/
            })
                .AddNewtonsoftJson(nt =>
                {
                    nt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            services.AddRouting(cfg => cfg.LowercaseUrls = true);

            services.AddDbContext<RiodeDBContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("cString"));
            });

            services.AddIdentity<RiodeUser, RiodeRole>()
                .AddEntityFrameworkStores<RiodeDBContext>();

            services.AddScoped<UserManager<RiodeUser>>();
            services.AddScoped<SignInManager<RiodeUser>>();
            services.AddScoped<RoleManager<RiodeRole>>();

            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.Password.RequireDigit = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredUniqueChars = 1;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 3;

                cfg.User.RequireUniqueEmail = true;
                cfg.Lockout.MaxFailedAccessAttempts = 3;
                cfg.Lockout.DefaultLockoutTimeSpan = new System.TimeSpan(0, 3, 0);

            });

          /*  services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/signin.html";
                cfg.AccessDeniedPath = "/accessdenied.html";

                cfg.ExpireTimeSpan = new System.TimeSpan(0, 5, 0);
                cfg.Cookie.Name = "Riode";
            });*/

            /*services.AddAuthentication();
            services.AddAuthorization();*/

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(assembly);



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

            app.UseRequestLocalization(cfg =>
            {
                cfg.AddSupportedCultures("az", "en");
                cfg.AddSupportedUICultures("az", "en");
                cfg.RequestCultureProviders.Clear();
                cfg.RequestCultureProviders.Add(new CultureProvider());
            });

            app.UseAuditMiddleware();

            /*app.UseAuthentication();
            app.UseAuthorization();*/


            app.UseEndpoints(endpoints =>
            {

                endpoints.MapGet("/comingsSoon.html", async (context) =>
                {
                    using (var sr = new StreamReader("views/Static/comingsSoon.html"))
                    {
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync(sr.ReadToEnd());
                    }
                }
                    );
                endpoints.MapControllerRoute(
                name: "areas-with-lang",
                pattern: "{lang}/{area:exists}/{controller=Dashboard}/{action=Index}/{id?}",
                constraints: new
                {
                    lang = "az|en|ru"
                }
                    );

                endpoints.MapControllerRoute(
                 name: "areas",
                 pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                     );
               /* endpoints.MapControllerRoute("default-signIn", "signin.html",
                    defaults: new
                    {
                        controller = "Account",
                        area = "",
                        action = "login"
                    }
                    );*/


                endpoints.MapControllerRoute("default-with-lang", "{lang}/{controller=home}/{action=index}/{id?}",
                       constraints: new
                       {
                           lang = "az|en|ru"
                       });
                endpoints.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
