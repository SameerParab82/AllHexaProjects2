using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAPI.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EventAPI.CustomFormatter;

namespace EventAPI
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
            services.AddDbContext<EventDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Event API",
                    Description = "Contains the API operations for adding, deleting and quering events",
                    Version = "1.0",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "Samee Parab", Email = "sameerparab82@gmail.com" }

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

            //Adding our JWT bearer token default authentication
            services.AddAuthentication(c =>
            {
                c.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(c =>
            {
                c.RequireHttpsMetadata = false;
                c.SaveToken = true;
                c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration.GetValue<string>("JWT:Issuer"),
                    ValidAudience = Configuration.GetValue<string>("JWT:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWT:Secret")))
                };

            })
            /*.AddGoogle(c=>
            {
                c.ClientId = "";
                c.ClientSecret = "";

            })*/
            ;

            services.AddMvc(c =>
            {
                //c.InputFormatters();//Custom input formatter
                c.OutputFormatters.Add(new CsvFormatter());
            })
            .AddXmlDataContractSerializerFormatters()//Accept header for XML type
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            InitalizeDatabase(app);

            app.UseAuthentication();//First Priority

            app.UseSwagger();

            app.UseCors("AllowAll");//Named Policy
            app.UseCors();//Default Policy

            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {

                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event API");
                    c.RoutePrefix = "";

                });
            }

            app.UseMvc();
        }

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
