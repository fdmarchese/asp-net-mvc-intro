using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_net_mvc_intro.Models;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_mvc_intro.Controllers
{
    public class RetornosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string EjemploString()
        {
            return "esto es un string que retorno";
        }

        public IActionResult EjemploStringConHtml()
        {
            return Content("<html><head></head><body style='background-color:yellow'>Prueba de string con contenido html</body></html>", "text/html");
        }

        public IActionResult EjemploJson()
        {
            Producto producto = new Producto
            {
                Id = Guid.NewGuid(),
                Descripcion = "Un producto de prueba",
                Orden = 1,
                Precio = 80M
            };

            return Json(producto);
        }

        public IActionResult EjemploVista()
        {
            return View();
        }

        public IActionResult EjemploVistaSinLayout()
        {
            return View();
        }
    }
}
