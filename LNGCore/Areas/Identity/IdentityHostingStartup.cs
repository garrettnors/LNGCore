using System;
using LNGCore.UI.Areas.Identity.Data;
using LNGCore.UI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(LNGCore.UI.Areas.Identity.IdentityHostingStartup))]
namespace LNGCore.UI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        private readonly IConfiguration _config;
        public IdentityHostingStartup(IConfiguration config)
        {
            _config = config;
        }
        public void Configure(IWebHostBuilder builder)
        {

            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(_config.GetSection("SiteConfiguration")["DbContext"]));

                services.AddScoped<SignInManager<LNGUser>, SignInManager<LNGUser>>();
                services.AddScoped<UserManager<LNGUser>, UserManager<LNGUser>>();
                services.AddScoped<IUserClaimsPrincipalFactory<LNGUser>, UserClaimsPrincipalFactory<LNGUser, IdentityRole>>();
                services.AddDefaultIdentity<LNGUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();
            });
        }
    }
}