using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cachingdemo
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMemoryCache();

            services.AddSingleton<StateDataService>();
            //services.AddTransient<StateDataService>();

            //services.AddDistributedMemoryCache();// In memory distributed cache

            services.AddDistributedSqlServerCache(config =>
                {
                    config.ConnectionString = Configuration.GetConnectionString("SqlConnection");
                    config.SchemaName = "dbo";
                    config.TableName = "CacheTable";
                }
                );
            //cmd to create cache table in SQl - dotnet sql-cache create "connectionstring"  "schemaname" "tablename"



            //for dotnet 2.2  Microsoft.Extension.caching.StackExchangeRedis(AddStackExchangeRedisCache)
            //for dotnet 2.1 Microsoft.Extension.caching.Redis(AddDistributedRedisCache)
            /* services.AddDistributedRedisCache(config =>
                 {
                     config.InstanceName = "redis";
                     config.Configuration = "localhost:6379";
             });*/

            //services.AddSession();
            services.AddSession(config => {

                config.Cookie.MaxAge = TimeSpan.FromSeconds(300);
                config.Cookie.Name = "Sameer Cookie";
                config.IdleTimeout = TimeSpan.FromMinutes(30);
            });


            //Cookies are default tempdata data provider.
            //AddCookieTempDataProvider -- not need to specify it. it is default method.
            services.AddMvc().AddSessionStateTempDataProvider()/*.AddCookieTempDataProvider()*/.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.ContainsKey("x-data"))
                {
                    context.Items["keyExists"] = true;
                    context.Items["DbName"] = "abc";
                }
                else
                {
                    context.Items["keyExists"] = false;
                    context.Items["DbName"] = "xyz";
                }
                await next.Invoke();
            });

            app.UseSession(); //session middleware

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
