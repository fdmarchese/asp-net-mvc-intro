using System;
using System.Collections.Generic;
using System.Linq;
using asp_net_mvc_intro.Models;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_mvc_intro.Controllers
{
    public class UsandoModelosController : Controller
    {
        private static List<Producto> _productos = new List<Producto>()
        {
            new Producto(Guid.NewGuid(), "Café", 1, 200M),
            new Producto(Guid.NewGuid(), "Té", 2, 65M),
            new Producto(Guid.NewGuid(), "Azúcar", 3, 30M),
            new Producto(Guid.NewGuid(), "Edulcorante", 4, 50M),
            new Producto(Guid.NewGuid(), "Yerba", 5, 120M),
            new Producto(Guid.NewGuid(), "Galletitas", 6, 100M),
            new Producto(Guid.NewGuid(), "Bizcochos", 7, 80M),
        };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListaDeProductos()
        {
            return View(_productos);
        }

        public IActionResult UnProducto(Guid id)
        {
            Producto producto = _productos.First(producto => producto.Id == id);
            return View(producto);
        }

        [HttpGet]
        public IActionResult AgregarProducto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AgregarProducto(Producto producto)
        {
            producto.Id = Guid.NewGuid();
            producto.Orden = _productos.Max(producto => producto.Orden) + 1;
            _productos.Add(producto);
            return RedirectToAction(nameof(ListaDeProductos));
        }

        [HttpGet]
        public IActionResult EditarProducto(Guid id)
        {
            Producto producto = _productos.First(producto => producto.Id == id);
            return View(producto);
        }

        // Este método debería ser PUT pero como usamos formularios html usamos POST
        [HttpPost]
        public IActionResult EditarProducto(Producto producto)
        {
            Producto productoExistente = _productos.First(prod => prod.Id == producto.Id);
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;

            return RedirectToAction(nameof(ListaDeProductos));
        }
    }
}
