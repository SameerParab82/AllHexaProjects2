using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace MIDDLEWAREDEMO.Middleware
{
    public class SecurityMiddleware
    {
        private RequestDelegate _next;

        public SecurityMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("<br/>Security Middleware request Processed");
            await _next.Invoke(context);
            await context.Response.WriteAsync("<br/>Security Middleware response Processed");

        }

    }

    public static class MiddlewareExtension
    {

        public static void UseSecurity(this IApplicationBuilder app)
        {
            app.UseMiddleware<SecurityMiddleware>();
        }
    }
}