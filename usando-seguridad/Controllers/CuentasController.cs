using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using usando_seguridad.Database;
using usando_seguridad.Models;

namespace usando_seguridad.Controllers
{
    [Authorize]
    public class CuentasController : Controller
    {
        private readonly SeguridadDbContext _context;

        public CuentasController(SeguridadDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Index()
        {
            var seguridadDbContext = _context.Cuentas.Include(c => c.Moneda).Include(c => c.Sucursal);
            return View(await seguridadDbContext.ToListAsync());
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Details(Guid? id)
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

        [Authorize(Roles = nameof(Rol.Administrador))]
        public IActionResult Create()
        {
            ViewData["MonedaId"] = new SelectList(_context.Monedas, "Id", "Codigo");
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Nombre");
            return View();
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero,Balance,SucursalId,MonedaId")] Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                cuenta.Id = Guid.NewGuid();
                _context.Add(cuenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MonedaId"] = new SelectList(_context.Monedas, "Id", "Codigo", cuenta.MonedaId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Nombre", cuenta.SucursalId);
            return View(cuenta);
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.Cuentas.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            ViewData["MonedaId"] = new SelectList(_context.Monedas, "Id", "Codigo", cuenta.MonedaId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Nombre", cuenta.SucursalId);
            return View(cuenta);
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Numero,Balance,SucursalId,MonedaId")] Cuenta cuenta)
        {
            if (id != cuenta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuenta);
                    await _context.SaveChangesAsync();
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
            return View(cuenta);
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
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

        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);
            _context.Cuentas.Remove(cuenta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpGet]
        public IActionResult MisCuentas()
        {
            var username = User.Identity.Name;
            var cliente = _context.Clientes
                .Include(cliente => cliente.Cuentas).ThenInclude(clienteCuenta => clienteCuenta.Cuenta)
                .FirstOrDefault(cliente => cliente.Username == username);

            return View(cliente);
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpGet]
        public IActionResult Depositar(Guid id)
        {
            var cuenta = _context.Cuentas.Find(id);
            return View(cuenta);
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpPost]
        public IActionResult Depositar(Guid id, decimal monto)
        {
            var cuenta = _context.Cuentas.Find(id);
            cuenta.Balance += monto;
            _context.SaveChanges();

            return RedirectToAction(nameof(MisCuentas));
        }

        private bool CuentaExists(Guid id)
        {
            return _context.Cuentas.Any(e => e.Id == id);
        }
    }
}
