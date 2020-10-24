using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ejemplo_cta_cte.Database;
using ejemplo_cta_cte.Models;
using Microsoft.AspNetCore.Authorization;

namespace ejemplo_cta_cte.Controllers
{
    public class CuentasController : Controller
    {
        private readonly CtaCteDbContext _context;

        public CuentasController(CtaCteDbContext context)
        {
            _context = context;
        }

        // GET: Cuentas
        public async Task<IActionResult> Index()
        {
            var ctaCteDbContext = _context.Cuentas.Include(c => c.Moneda).Include(c => c.Sucursal);
            return View(await ctaCteDbContext.ToListAsync());
        }

        // GET: Cuentas/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = _context.Cuentas
                .Include(cuenta => cuenta.Moneda)
                .Include(cuenta => cuenta.Sucursal)
                .Include(cuenta => cuenta.Clientes).ThenInclude(clienteCuenta => clienteCuenta.Cliente)
                .FirstOrDefault(cuenta => cuenta.Id == id);

            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // GET: Cuentas/Create
        public IActionResult Create()
        {
            ViewData["MonedaId"] = new SelectList(_context.Monedas, "Id", "Codigo");
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, nameof(Sucursal.Id), nameof(Sucursal.NombreYDireccion));
            ViewData["Clientes"] = new MultiSelectList(_context.Clientes, nameof(Cliente.Id), nameof(Cliente.Descripcion));
            return View();
        }

        // POST: Cuentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cuenta cuenta, Guid[] clienteIds)
        {
            if (ModelState.IsValid)
            {
                cuenta.Id = Guid.NewGuid();

                foreach (Guid clienteId in clienteIds)
                {
                    var cliente = new ClienteCuenta()
                    {
                        Id = Guid.NewGuid(),
                        ClienteId = clienteId,
                        CuentaId = cuenta.Id
                    };

                    _context.Add(cliente);
                }

                _context.Add(cuenta);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MonedaId"] = new SelectList(_context.Monedas, "Id", "Codigo", cuenta.MonedaId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "NombreYDireccion", cuenta.SucursalId);
            ViewData["Clientes"] = new MultiSelectList(_context.Clientes, nameof(Cliente.Id), nameof(Cliente.Dni), clienteIds);

            return View(cuenta);
        }

        // GET: Cuentas/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = _context.Cuentas
                .Include(cuenta => cuenta.Clientes)
                .FirstOrDefault(cuenta => cuenta.Id == id);

            if (cuenta == null)
            {
                return NotFound();
            }

            var clienteIds = cuenta.Clientes.Select(clienteCuenta => clienteCuenta.ClienteId);

            ViewData["MonedaId"] = new SelectList(_context.Monedas, "Id", "Codigo", cuenta.MonedaId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Nombre", cuenta.SucursalId);
            ViewData["Clientes"] = new MultiSelectList(_context.Clientes, nameof(Cliente.Id), nameof(Cliente.Dni), clienteIds);

            return View(cuenta);
        }

        // POST: Cuentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Cuenta cuenta, Guid[] clienteIds)
        {
            if (id != cuenta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtengo la cuenta desde la base de datos
                    var clienteCuentas = _context.ClienteCuentas.Where(clienteCuenta => clienteCuenta.CuentaId == id);

                    _context.RemoveRange(clienteCuentas);

                    // Crea todas las relaciones con Clientes para la cuenta editada.
                    foreach (Guid clienteId in clienteIds)
                    {
                        var cliente = new ClienteCuenta()
                        {
                            Id = Guid.NewGuid(),
                            ClienteId = clienteId,
                            CuentaId = cuenta.Id
                        };

                        _context.Add(cliente);
                    }

                    _context.Update(cuenta);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentaExists(cuenta.Id))
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
            ViewData["MonedaId"] = new SelectList(_context.Monedas, "Id", "Codigo", cuenta.MonedaId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Nombre", cuenta.SucursalId);
            ViewData["Clientes"] = new MultiSelectList(_context.Clientes, nameof(Cliente.Id), nameof(Cliente.Dni), clienteIds);
            return View(cuenta);
        }

        // GET: Cuentas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.Cuentas
                .Include(c => c.Moneda)
                .Include(c => c.Sucursal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // POST: Cuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);
            _context.Cuentas.Remove(cuenta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuentaExists(Guid id)
        {
            return _context.Cuentas.Any(e => e.Id == id);
        }
    }
}
