using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TP03_WebApp.Entidades;
using TP03_WebApp.Models;
using TP03_WebApp.Models.DB;
using NLog.Web;

namespace TP03_WebApp
{
    public class Startup
    {
        private const int IdleTimeoutEnSegundos = 1200;
        static readonly DBTemp DB = new DBTemp(NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger());
        public Startup(IConfiguration configuration)
        {
           
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSingleton(DB);
            services.AddSingleton<ICadeteDB>(new RepositorioCadete(Configuration.GetConnectionString("Default"), NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger()));
            services.AddSingleton<IClienteDB>(new RepositorioCliente(Configuration.GetConnectionString("Default"), NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger()));
            services.AddSingleton<IPedidoDB>(new RepositorioPedido(Configuration.GetConnectionString("Default"), NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger()));
            IUsuarioDB repoUsuario = new RepositorioUsuario(Configuration.GetConnectionString("Default"), NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger());
            services.AddSingleton(repoUsuario);
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromSeconds(IdleTimeoutEnSegundos);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });
            services.AddAutoMapper(typeof(PerfilDeMapeo));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

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
