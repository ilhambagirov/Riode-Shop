using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var scope = httpContext.RequestServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RiodeDBContext>();

                var routeDate = httpContext.GetRouteData();

                var log = new AuditLog();
                log.CreatedDate = log.RequestTime = DateTime.Now;
                log.IsHttps = httpContext.Request.IsHttps;
                log.Method = httpContext.Request.Method;
                log.Path = httpContext.Request.Path;

                if (routeDate.Values.TryGetValue("area", out object area))
                {
                    log.Area = area.ToString();
                }
                if (routeDate.Values.TryGetValue("controller", out object controller))
                {
                    log.Controller = controller.ToString();
                }
                if (routeDate.Values.TryGetValue("action", out object action))
                {
                    log.Action = action.ToString();
                }

                if (!string.IsNullOrWhiteSpace(httpContext.Request.QueryString.Value))
                {
                    log.QueryString = httpContext.Request.QueryString.Value;
                }

                await _next(httpContext);

                log.StatuCode = httpContext.Response.StatusCode;
                log.ResponseTime = DateTime.Now;

                db.AuditLogs.Add(log);
                db.SaveChanges();
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuditMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuditMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuditMiddleware>();
        }
    }
}
