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
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Diagnostics;

namespace FirstMvcCore
{
    public class Startup
    {
        static Dictionary<string, string> appSetting = new Dictionary<string, string>()
        {
            {"Name","Sameer Parab" },
            {"Email","SameerParab82@gmail.com" }
        };
        public Startup(IConfiguration configuration)
        {

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddInMemoryCollection(appSetting)
                                            .AddXmlFile("mysetting.xml", optional: true)
                                            .AddIniFile("mysetting.ini", optional: true)
                                            .AddJsonFile("mysetting.json", optional: true)
                                            .AddKeyPerFile("FileDir", optional: true)
                                            .AddJsonFile("appsettings.json", optional: true)
                                            .AddEnvironmentVariables();
            //Configuration = configuration;
            Configuration = configBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine(Configuration.GetValue<string>("Company"));
            Console.WriteLine(Configuration.GetValue<string>("Address:City"));
            Console.WriteLine(Configuration.GetValue<string>("ProjectName"));
            Console.WriteLine(Configuration.GetValue<int>("Duration"));
            Console.WriteLine(Configuration.GetValue<string>("Course:Title"));
            Console.WriteLine(Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT"));
            Console.WriteLine(Configuration.GetValue<string>("Name"));
            Console.WriteLine(Configuration.GetValue<string>("Message"));

            var course = Configuration.GetSection("Course");
            Console.WriteLine(course["Title"]);

            services.Configure<AppConfigModel>(Configuration);




            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            env.EnvironmentName = "Production";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");

                app.UseExceptionHandler((builder) =>
                    {
                        builder.Run(async (context) =>
                        {

                            context.Response.StatusCode = 500;
                            context.Response.Headers["Content-Type"] = "text/html";
                            await context.Response.WriteAsync("<h2>Some Error Occured</h2>");
                            var exceptionPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                            await context.Response.WriteAsync($"<p>{exceptionPathFeature.Error.Message}");

                        });

                    });
                app.UseHsts();
            }

            //app.UseStatusCodePages(); -- default status error mesg


            app.UseStatusCodePages("text/html", "Client side errorn occured with status code {0}"); // With arguments

            app.UseStatusCodePagesWithRedirects("/index.html");

            app.UseStatusCodePagesWithRedirects("/index.html?code={0}");// Client side redirection

            app.UseStatusCodePagesWithReExecute("/index.html?code={0}");// Server side redirection. Browser doesn't request again. its server who redirects user within server itself.

            app.UseHttpsRedirection();

            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("default.html");
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);


            app.UseStaticFiles();// Used to configure static files from wwwroot
            //app.UseStaticFiles(new StaticFileOptions {
            //    RequestPath = "/docs",
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"mydocs"))

            //});

            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    RequestPath = "/docs",
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "mydocs"))
            //});


            //combination of static files, directorybrowser, and default files
            var fileOptions = new FileServerOptions
            {
                RequestPath = "/mydocs",
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "mydocs")),
                EnableDirectoryBrowsing = true,

            };
            //fileOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileOptions.DefaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseFileServer(fileOptions);


            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
