using System;
using System.Collections.Generic;
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
    [Authorize(Roles = nameof(Rol.Administrador))]
    public class ClienteCuentasController : Controller
    {
        private readonly SeguridadDbContext _context;

        public ClienteCuentasController(SeguridadDbContext context)
        {
            _context = context;
        }

        // GET: ClienteCuentas
        public async Task<IActionResult> Index()
        {
            var seguridadDbContext = _context.ClienteCuentas.Include(c => c.Cliente).Include(c => c.Cuenta);
            return View(await seguridadDbContext.ToListAsync());
        }

        // GET: ClienteCuentas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteCuenta = await _context.ClienteCuentas
                .Include(c => c.Cliente)
                .Include(c => c.Cuenta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clienteCuenta == null)
            {
                return NotFound();
            }

            return View(clienteCuenta);
        }

        // GET: ClienteCuentas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido");
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "Id", "Numero");
            return View();
        }

        // POST: ClienteCuentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CuentaId,ClienteId")] ClienteCuenta clienteCuenta)
        {
            if (ModelState.IsValid)
            {
                clienteCuenta.Id = Guid.NewGuid();
                _context.Add(clienteCuenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", clienteCuenta.ClienteId);
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "Id", "Numero", clienteCuenta.CuentaId);
            return View(clienteCuenta);
        }

        // GET: ClienteCuentas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteCuenta = await _context.ClienteCuentas.FindAsync(id);
            if (clienteCuenta == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", clienteCuenta.ClienteId);
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "Id", "Numero", clienteCuenta.CuentaId);
            return View(clienteCuenta);
        }

        // POST: ClienteCuentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CuentaId,ClienteId")] ClienteCuenta clienteCuenta)
        {
            if (id != clienteCuenta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clienteCuenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteCuentaExists(clienteCuenta.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", clienteCuenta.ClienteId);
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "Id", "Numero", clienteCuenta.CuentaId);
            return View(clienteCuenta);
        }

        // GET: ClienteCuentas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteCuenta = await _context.ClienteCuentas
                .Include(c => c.Cliente)
                .Include(c => c.Cuenta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clienteCuenta == null)
            {
                return NotFound();
            }

            return View(clienteCuenta);
        }

        // POST: ClienteCuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var clienteCuenta = await _context.ClienteCuentas.FindAsync(id);
            _context.ClienteCuentas.Remove(clienteCuenta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteCuentaExists(Guid id)
        {
            return _context.ClienteCuentas.Any(e => e.Id == id);
        }
    }
}
