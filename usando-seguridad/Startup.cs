using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using usando_seguridad.Database;

namespace usando_seguridad
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
            // Habilitar la autenticación por cookie
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                // Especificamos que la ruta para login es "/Cuentas/Ingresar" 
                // => esto quiere decir que si alguien que no está autenticado intenta ingresar será enviado a 
                // la página de login ubicada en el controlador de Cuentas, el action method Ingresar.
                options.LoginPath = "/Accesos/Ingresar"; // ruta relativa para login.
                options.AccessDeniedPath = "/Accesos/NoAutorizado"; // ruta relativa para accesos no autorizados por falta de permisos en el rol.
                options.LogoutPath = "/Accesos/Salir"; // ruta relativa para logout
            });

            services.AddControllersWithViews();

            services.AddDbContext<SeguridadDbContext>(options => options.UseSqlite("filename=seguridad.db"));
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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Se debe agregar para que la aplicación utilice el contexto de autenticación.
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Esta sección debe ir aquí (Después de app.UseMvc() si queremos utilizar TempData en la aplicación.
            app.UseCookiePolicy();
        }
    }
}
