using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MIDDLEWAREDEMO.Middleware;

namespace Middlewaredemo {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices (IServiceCollection services) { }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

           // app.UseMiddleware<SecurityMiddleware>();
            app.UseSecurity();

            app.Use (async (context, next) => {
               // context.Response.Headers.Add ("Content-Type", "text/html");
                await context.Response.WriteAsync ("<br/> MI request Processed");
                await next.Invoke ();
                await context.Response.WriteAsync ("<br/>MI response Processed");

            });

            app.Use (async (context, next) => {

                await context.Response.WriteAsync ("<br/> M2 request Processed");
                await next.Invoke ();
                await context.Response.WriteAsync ("<br/>M2 response Processed");

            });

            app.Map ("/about", (builder) => {
                builder.Use (async (context, next) => {
                    await context.Response.WriteAsync ("<br/> MI for About request Processed");
                    await next.Invoke ();
                    await context.Response.WriteAsync ("<br/>MI for About response Processed");

                });
                builder.Run (async (context) => {
                    await context.Response.WriteAsync ("<br/>About page");

                });

            });

            app.Map ("/contact", (builder) => {
                builder.Use (async (context, next) => {
                    await context.Response.WriteAsync ("<br/> MI for Contact request Processed");
                    await next.Invoke ();
                    await context.Response.WriteAsync ("<br/>MI for Contact response Processed");

                });
                builder.Run (async (context) => {
                    await context.Response.WriteAsync ("<br/>Contact page");

                });

            });

            app.MapWhen (ctx => ctx.Request.Query["version"] == "v1", (builder) => {

                builder.Run (async (context) => {
                    await context.Response.WriteAsync ("<br/>Version  v1 executed");

                });
            });

            app.MapWhen (ctx => ctx.Request.Query["version"] == "v2", (builder) => {
                builder.Use (async (context, next) => {
                    await context.Response.WriteAsync ("<br/> MI for Version2  request Processed");
                    await next.Invoke ();
                    await context.Response.WriteAsync ("<br/>MI for Version2  response Processed");

                });
                builder.Run (async (context) => {
                    await context.Response.WriteAsync ("<br/>Version  v2 executed");

                });
            });

            app.Run (async (context) => {
                await context.Response.WriteAsync ("<br/>Hello World!");
            });
        }
    }
}