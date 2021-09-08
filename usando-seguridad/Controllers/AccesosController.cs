using System;
using System.Linq;
using System.Security.Claims;
using usando_seguridad.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using usando_seguridad.Database;
using usando_seguridad.Extensions;

namespace usando_seguridad.Controllers
{
    [AllowAnonymous]
    public class AccesosController : Controller
    {
        private readonly SeguridadDbContext _context;
        private const string _Return_Url = "ReturnUrl";

        public AccesosController(SeguridadDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Ingresar(string returnUrl)
        {
            // Guardamos la url de retorno para que una vez concluído el login del 
            // usuario lo podamos redirigir a la página en la que se encontraba antes
            TempData[_Return_Url] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Ingresar(string username, string password, Rol rol)
        {
            string returnUrl = TempData[_Return_Url] as string;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                Usuario usuario = null;

                if (rol == Rol.Cliente)
                {
                    usuario = _context.Clientes.FirstOrDefault(cliente => cliente.Username == username);
                }
                else if (rol == Rol.Administrador)
                {
                    usuario = _context.Administradores.FirstOrDefault(administrador => administrador.Username == username);
                }

                if (usuario != null)
                {
                    var passwordEncriptada = password.Encriptar();

                    if (usuario.Password.SequenceEqual(passwordEncriptada))
                    {
                        // Se crean las credenciales del usuario que serán incorporadas al contexto
                        ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                        // El lo que luego obtendré al acceder a User.Identity.Name
                        identity.AddClaim(new Claim(ClaimTypes.Name, username));

                        // Se utilizará para la autorización por roles
                        identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol.ToString()));

                        // Lo utilizaremos para acceder al Id del usuario que se encuentra en el sistema.
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));

                        // Lo utilizaremos cuando querramos mostrar el nombre del usuario logueado en el sistema.
                        identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));

                        identity.AddClaim(new Claim(nameof(Usuario.Foto), usuario.Foto ?? string.Empty));

                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                        // En este paso se hace el login del usuario al sistema
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();

                        TempData["LoggedIn"] = true;

                        if (!string.IsNullOrWhiteSpace(returnUrl))
                            return Redirect(returnUrl);

                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
            }

            // Completo estos dos campos para poder retornar a la vista en caso de errores.
            ViewBag.Error = "Usuario o contraseña incorrectos";
            ViewBag.UserName = username;
            TempData[_Return_Url] = returnUrl;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Salir()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult NoAutorizado()
        {
            return View();
        }
    }
}
