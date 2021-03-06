using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.EntityFrameworkCore.Extensions;
using SISCOSMAC.DAL.DbContextSql;
using SISCOSMAC.DAL.UFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISCOSMAC.Web
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddDbContext<ContextSql>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("ConnectionDefauld"));
            });

            var sqlConexion = Configuration.GetConnectionString("ConnectionDefauld");
            services.AddEntityFrameworkMySQL().AddDbContext<ContextSql>(options => options.UseMySQL(sqlConexion));
            services.AddTransient<IUnitOfWork, UnitOfWork>();


            //CookieAuthentication
            services.AddAuthentication("SiscosmacCookieAuthenticationSheme")
                .AddCookie("SiscosmacCookieAuthenticationSheme", opciones => {
                    opciones.LoginPath = "/Acceso/Login";
                    opciones.AccessDeniedPath = "/Home/AccesoNegado";
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }



            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseRouting();

            
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
