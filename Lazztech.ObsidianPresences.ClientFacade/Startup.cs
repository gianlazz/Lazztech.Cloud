using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lazztech.ObsidianPresences.ClientFacade.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using HackathonManager.Interfaces;
using Microsoft.AspNetCore.Routing;
using HackathonManager.Sms;
using Lazztech.ObsidianPresences.ClientFacade.Hubs;

namespace Lazztech.ObsidianPresences.ClientFacade
{
    public class Startup
    {
        public static HackathonManager.RepositoryPattern.IRepository DbRepo = HackathonManager.DIContext.Context.GetMLabsMongoDbRepo();
        public static ISmsService SmsService = HackathonManager.DIContext.Context.GetTwilioSmsService();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //HackathonManager.SmsDaemon.Program._responder = new Util.Responder();
            //HackathonManager.SmsDaemon.Program.Main(new string[] { });
            //AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var twillioConfigSection = Configuration.GetSection("TwilioCredentials");
            TwilioCredentials.accountSid = twillioConfigSection["AccountSid"];
            TwilioCredentials.authToken = twillioConfigSection["AuthToken"];
            TwilioCredentials.fromTwilioNumber = twillioConfigSection["TwilioFromNumber"];

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // SqlServer Entity Framework Data Adapter Setup
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // PostgreSql Entity Framework Data Adapter Setup
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseNpgsql(
            //        Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Simple demo", Version = "v1" });
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //var xmlPath = Path.Combine(basePath, $"{PlatformServices.Default.Application.ApplicationName}.xml");
                //c.IncludeXmlComments(xmlPath);
            });


            services.AddMvc()
                    .AddRazorPagesOptions(options =>
                    {
                        //options.Conventions.AuthorizeFolder("/Cogneat");
                        //options.Conventions.AuthorizeFolder("/Crawler");
                        //options.Conventions.AuthorizeFolder("/Events");
                        //options.Conventions.AuthorizeFolder("/Garden");
                        //options.Conventions.AuthorizeFolder("/Notifications");
                        //options.Conventions.AuthorizeFolder("/Vision");

                        //options.Conventions.AllowAnonymousToPage("/Private/PublicPage");
                        //options.Conventions.AllowAnonymousToFolder("/Private/PublicPages");
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<IdentityUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My simple API"); });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            var defaultAdminSection = Configuration.GetSection("DefaultAdminUser");
            var userName = defaultAdminSection["Username"];
            var email = defaultAdminSection["Email"];
            var password = defaultAdminSection["Password"];
            ApplicationDbInitializer.SeedUser(userManager, userName, email, password);
        }
    }
}
