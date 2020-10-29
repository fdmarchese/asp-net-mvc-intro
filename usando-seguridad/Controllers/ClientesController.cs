using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using usando_seguridad.Database;
using usando_seguridad.Extensions;
using usando_seguridad.Models;

namespace usando_seguridad.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly SeguridadDbContext _context;

        public ClientesController(SeguridadDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente cliente, string pass)
        {
            try
            {
                pass.ValidarPassword();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Cliente.Password), ex.Message);
            }

            if (ModelState.IsValid)
            {
                cliente.Id = Guid.NewGuid();
                cliente.Password = pass.Encriptar();
                _context.Add(cliente);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Cliente cliente, string pass)
        {
            if (!string.IsNullOrWhiteSpace(pass))
            {
                try
                {
                    pass.ValidarPassword();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Cliente.Password), ex.Message);
                }
            }

            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var clienteDatabase = _context.Clientes.Find(id);

                    clienteDatabase.Nombre = cliente.Nombre;
                    clienteDatabase.Apellido = cliente.Apellido;
                    clienteDatabase.FechaNacimiento = cliente.FechaNacimiento;
                    clienteDatabase.Username = cliente.Username;

                    if (!string.IsNullOrWhiteSpace(pass))
                    {
                        clienteDatabase.Password = pass.Encriptar();
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpGet]
        public IActionResult EditMe()
        {
            var username = User.Identity.Name;
            var cliente = _context.Clientes.FirstOrDefault(cliente => cliente.Username == username);

            return View(cliente);
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpPost]
        public IActionResult EditMe(Cliente cliente, string pass)
        {
            if (!string.IsNullOrWhiteSpace(pass))
            {
                try
                {
                    pass.ValidarPassword();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Cliente.Password), ex.Message);
                }
            }

            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                var clienteDatabase = _context.Clientes.FirstOrDefault(cliente => cliente.Username == username);

                clienteDatabase.Nombre = cliente.Nombre;
                clienteDatabase.Apellido = cliente.Apellido;
                clienteDatabase.Dni = cliente.Dni;
                clienteDatabase.FechaNacimiento = cliente.FechaNacimiento;

                if (!string.IsNullOrWhiteSpace(pass))
                {
                    clienteDatabase.Password = pass.Encriptar();
                }

                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(cliente);
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(Guid id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
