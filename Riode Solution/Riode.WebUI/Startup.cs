using MediatR;
using Microsoft.AspNetCore.Authentication;
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
using Riode.Application.Core.Extensions;
using Riode.Application.Core.Middlewares;
using Riode.Application.Core.Provider;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities.Membership;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Riode.WebUI
{
    public class Startup
    {
        //test
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

                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));
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

            services.AddScoped<IClaimsTransformation, AppClaimProvider>();

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

            services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/signin.html";
                cfg.AccessDeniedPath = "/accessdenied.html";

                cfg.ExpireTimeSpan = new System.TimeSpan(0, 5, 0);
                cfg.Cookie.Name = "Riode";
            });

            services.AddAuthentication();
            services.AddAuthorization(cfg =>
            {
                foreach (var item in Extension.principlies)
                {
                    cfg.AddPolicy(item, p =>
                    {
                        p.RequireAssertion(assertion =>
                        {
                            return
                            assertion.User.IsInRole("SuperAdmin") ||
                            assertion.User.HasClaim(c => c.Type.Equals(item) && c.Value.Equals("1"));

                        });
                    });

                }

            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            var asmbls=AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Riode")).ToArray();
            services.AddMediatR(asmbls);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.SeedMembership();
            app.UseStaticFiles();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                if (!context.Request.Cookies.ContainsKey("Riode") && context.Request.RouteValues.TryGetValue("area", out object area)
                && area.ToString().ToLower().Equals("admin")
                )
                {
                    var attr = context.GetEndpoint().Metadata.GetMetadata<AllowAnonymousAttribute>();
                    if (attr == null)
                    {
                        context.Response.Redirect("/admin/signin.html");
                        await context.Response.CompleteAsync();
                    }
                }
                await next();
            });

            app.UseRequestLocalization(cfg =>
            {
                cfg.AddSupportedCultures("az", "en");
                cfg.AddSupportedUICultures("az", "en");
                cfg.RequestCultureProviders.Clear();
                cfg.RequestCultureProviders.Add(new CultureProvider());
            });

            app.UseAuditMiddleware();

            app.UseAuthentication();
            app.UseAuthorization();


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

                endpoints.MapControllerRoute("admin-signIn", "admin/signin.html",
                defaults: new
                {
                    controller = "Account",
                    area = "Admin",
                    action = "login"
                }
                );
                endpoints.MapControllerRoute("admin-signout", "admin/signout.html",
               defaults: new
               {
                   controller = "Account",
                   area = "Admin",
                   action = "login"
               }
               );
                endpoints.MapControllerRoute("default-signIn", "signin.html",
                    defaults: new
                    {
                        controller = "Account",
                        area = "",
                        action = "login"
                    }
                    );
                endpoints.MapControllerRoute("default-signIn", "register.html",
                   defaults: new
                   {
                       controller = "Account",
                       area = "",
                       action = "register"
                   }
                   );

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
