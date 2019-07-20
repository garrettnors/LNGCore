using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Implementations;
using LNGCore.Domain.Services.Interfaces;
using LNGCore.UI;
using LNGCore.UI.Areas.Identity.Data;
using LNGCore.UI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rotativa.AspNetCore;

namespace LNGCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //a secrets.json file will contain some private keys and connection strings that I wish to hide from the public repo.
            var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
            RotativaConfiguration.Setup(env);
            Configuration = configurationBuilder.Build();
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
            services.AddSession();

            //services.AddAuthentication(options => 
            //{
            //    options.DefaultAuthenticateScheme = "";
            //});
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHttpsRedirection(options => { options.HttpsPort = 443; });

            services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(Configuration.GetSection("SiteConfiguration")["DbContext"]));

            services.AddScoped<SignInManager<LNGUser>, SignInManager<LNGUser>>();
            services.AddScoped<UserManager<LNGUser>, UserManager<LNGUser>>();
            services.AddScoped<IUserClaimsPrincipalFactory<LNGUser>, UserClaimsPrincipalFactory<LNGUser, IdentityRole>>();
            services.AddDefaultIdentity<LNGUser>()
            .AddDefaultUI(UIFramework.Bootstrap4)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>();

            services.AddScoped<IBillSheetService, BillSheetService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<LngDbContext, LngDbContext>();
            services.AddSingleton(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
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
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
