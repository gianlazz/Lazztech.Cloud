using HackathonManager.MongoDB;
using HackathonManager.Sms;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Cloud.ClientFacade.Hubs;
using Lazztech.Cloud.ClientFacade.Util;
using Lazztech.Events.Domain;
using Lazztech.Events.Dto.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;

namespace Lazztech.Cloud.ClientFacade
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public static IRepository DbRepo;
        public static ISmsService SmsService;
        public static IRequestNotifier Responder = new SignalRNotifier();
        public static IMongoDatabase Db;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //RequestConductor = new MentorRequestConductor(DbRepo, SmsService, Responder);
        }

        private void SetupMongoDBClient()
        {
            var connectionString = Configuration.GetConnectionString("MongoDBConnection");
            var client = new MongoClient(connectionString);
            Db = client.GetDatabase("hackathonmanager");

            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);

            DbRepo = HackathonManager.DIContext.Context.GetMLabsMongoDbRepo(connectionString);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
            });

            services.AddMvc()
                    .AddRazorPagesOptions(options =>
                    {
                        //options.Conventions.AuthorizeFolder("/Cogneat");
                        //options.Conventions.AuthorizeFolder("/Crawler");
                        options.Conventions.AuthorizeFolder("/Events");
                        //options.Conventions.AuthorizeFolder("/Garden");
                        //options.Conventions.AuthorizeFolder("/Notifications");
                        options.Conventions.AuthorizeFolder("/Vision", "RequireAdministratorRole");

                        //options.Conventions.AllowAnonymousToPage("/Private/PublicPage");
                        //options.Conventions.AllowAnonymousToFolder("/Private/PublicPages");
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();

            //public static IRepository DbRepo;
            //public static ISmsService SmsService;
            //public static IRequestNotifier Responder = new SignalRNotifier();
            //public static IMongoDatabase Db;


            var twillioConfigSection = Configuration.GetSection("TwilioCredentials");
            var accountSid = twillioConfigSection["AccountSid"];
            var authToken = twillioConfigSection["AuthToken"];
            var fromTwilioNumber = twillioConfigSection["TwilioFromNumber"];
            //SmsService = HackathonManager.DIContext.Context.GetTwilioSmsService(accountSid, authToken, fromTwilioNumber);

            services.AddScoped<ISmsService>(s => new TwilioSmsService(accountSid, authToken, fromTwilioNumber));
            services.AddScoped<IRepository, MongoRepository>();
            services.AddScoped<IRequestNotifier, SignalRNotifier>();
            services.AddSingleton<IMentorRequestConductor, MentorRequestConductor>();

            var provider = services.BuildServiceProvider();
            var sms = provider.GetService<ISmsService>();
            provider.GetRequiredService<IRepository>();
            provider.GetRequiredService<IRepository>();
            provider.GetRequiredService<IMentorRequestConductor>();

            sms.SendSms("4254434290", "this is a test");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.AddEfDiagrams<ApplicationDbContext>();
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
                routes.MapHub<ProgressHub>("/progressHub");
            });

            var defaultAdminSection = Configuration.GetSection("DefaultAdminUser");
            var email = defaultAdminSection["Email"];
            var password = defaultAdminSection["Password"];
            ApplicationDbInitializer.SeedAdminUser(userManager, roleManager, email, password);
        }
    }
}