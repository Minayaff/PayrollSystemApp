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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HrPayroll.DAL;
using HrPayroll.Controllers;
using HrPayroll.Models;

namespace HrPayroll
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(AccountController));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:Default"]);

            });

            services.AddIdentity<AppUser, IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultUI()
             .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(identityoptions =>
            {
                identityoptions.Password.RequireDigit = false;
                identityoptions.Password.RequireLowercase = false;
                identityoptions.Password.RequireUppercase = false;
                identityoptions.Lockout.MaxFailedAccessAttempts = 3;
                identityoptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                identityoptions.Password.RequiredLength = 8;
                identityoptions.User.RequireUniqueEmail = true;

            });



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
