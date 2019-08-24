using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndentityAPI.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IndentityAPI
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
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Identity API",
                    Description = "User Authentication API methods",
                    Version = "1.0",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "Sameer Parab", Email = "sameerparab82@gmail.com" }
                });
            });

            services.AddCors(c =>
            {
                c.AddPolicy("HexClient", config =>
                {
                    config.WithOrigins("https://microsoft.com", "https://synergetics.com")
                    .WithHeaders("Content-Type", "Accept", "Authorization")
                    .WithMethods("GET", "POST", "PUT", "DELETE");

                    config.WithOrigins("https://infosys.com")
                    .WithMethods("GET");

                });
                c.AddPolicy("AllowAll", config =>
                {
                    config.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
                c.AddDefaultPolicy(config =>
                {
                    config.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
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
            InitalizeDatabase(app);

            app.UseSwagger();

            app.UseCors("AllowAll");//Named Policy
            app.UseCors();//Default Policy

            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {

                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API");
                    c.RoutePrefix = "";

                });
            }

            app.UseMvc();
        }


        private void InitalizeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityDbContext>())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
