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
using Microsoft.EntityFrameworkCore;

namespace EventMangementApp
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

            services.AddDbContext<EventDbContext>(options=> {

                //options.UseInMemoryDatabase(databaseName: "EventDb");
                options.UseSqlServer(Configuration.GetConnectionString("EventSqlConnection"));

            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

            InitalizeDatabase(app);//


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

      /*  private void InitalizeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                using(var db= new EventDbContext(serviceProvider.GetRequiredService<DbContextOptions<EventDbContext>>()))
                {

                    if(db.Events.Any())
                    {
                        return;
                    }
                    else
                    {

                        db.Events.Add(new Models.EventInfo {
                            Title="Azure CosmosDb",
                            StartDate=DateTime.Now.AddDays(5),
                            EndDate=DateTime.Now.AddDays(10),
                            Location="Mumbai",
                            Organizer="Sameer Parab",
                            RegistrationURL="https://events.microsoft.com/MDAZ001"
                        });
                        db.Events.Add(new Models.EventInfo
                        {
                            Title = "Oracle CosmosDb",
                            StartDate = DateTime.Now.AddDays(15),
                            EndDate = DateTime.Now.AddDays(12),
                            Location = "Pune",
                            Organizer = "Brock Lesnar",
                            RegistrationURL = "https://events.microsoft.com/MDAZ001"
                        });
                        db.Events.Add(new Models.EventInfo
                        {
                            Title = "MySql CosmosDb",
                            StartDate = DateTime.Now.AddDays(2),
                            EndDate = DateTime.Now.AddDays(0),
                            Location = "Nagpur",
                            Organizer = "Triple H",
                            RegistrationURL = "https://events.microsoft.com/MDAZ001"
                        });
                        db.SaveChanges();
                    }
                }
            }
        }*/

        private void InitalizeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<EventDbContext>())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
