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
using WebAdmin.Models;
using Microsoft.AspNetCore.Identity;
using WebAdmin.Services;
using WebAdmin.Areas.Identity.Data;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace WebAdmin
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

            services.AddDistributedMemoryCache();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(15);//You can set Time   
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<DBAdminContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("dbAdminDatabase")));


            

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddMvc();

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
            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {

            
                //routes.MapRoute(
                //    name: "default",
                //    template: "{Identity=Pages}/{action=Login}/{id?}");

                routes.MapRoute(
                    name: "Home",
                    template: "{controller=Home}/{action=Index}/{id?}");
                


            });
            Rotativa.AspNetCore.RotativaConfiguration.Setup(env);
           
          
        }
    }
}
