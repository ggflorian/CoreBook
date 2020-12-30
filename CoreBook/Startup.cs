using CoreBook.DataAccess.Data;
using CoreBook.DataAccess.Repository;
using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBook
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // FG Error: DefIdentity doesn't support role management added in Register.cs
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>(); 

            // FG Add
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); // commented token generated

            services.AddSingleton<IEmailSender, EmailSender>(); // for email sender error
            services.Configure<EmailOptions>(Configuration); // user to pass param to EmailOptions in CoreBook.Utility

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.ConfigureApplicationCookie(options => // for Identity pages on redirect users who are not authenticated
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.AddAuthentication().AddFacebook(opt =>
            {
                opt.AppId = "298909884868494";
                opt.AppSecret = "f4738ec1668cecc84f56ae938390463c";
            });
            services.AddAuthentication().AddGoogle(opt =>
            {
                opt.ClientId = "500207077195-nh58246ic5qd2t44ifffvp3hocgt8n0g.apps.googleusercontent.com";
                opt.ClientSecret = "w8L99NbzCWJrFWzz_uqS65wz";
            });
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(32);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
