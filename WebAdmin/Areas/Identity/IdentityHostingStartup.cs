using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAdmin.Areas.Identity.Data;
using WebAdmin.Models;

[assembly: HostingStartup(typeof(WebAdmin.Areas.Identity.IdentityHostingStartup))]
namespace WebAdmin.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<WebAdminContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("WebAdminContextConnection")));

                services.AddDefaultIdentity<WebAdminUser>()
                    .AddEntityFrameworkStores<WebAdminContext>();
            });
        }
    }
}