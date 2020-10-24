using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ejemplo_cta_cte.Database;
using ejemplo_cta_cte.Models;

namespace ejemplo_cta_cte.Controllers
{
    public class ClienteCuentasController : Controller
    {
        private readonly CtaCteDbContext _context;

        public ClienteCuentasController(CtaCteDbContext context)
        {
            _context = context;
        }

        // GET: ClienteCuentas
        public async Task<IActionResult> Index()
        {
            var ctaCteDbContext = _context.ClienteCuentas.Include(c => c.Cliente).Include(c => c.Cuenta);
            return View(await ctaCteDbContext.ToListAsync());
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
        public IActionResult Create([Bind("Id,CuentaId,ClienteId")] ClienteCuenta clienteCuenta)
        {
            if (ModelState.IsValid)
            {
                clienteCuenta.Id = Guid.NewGuid();
                _context.Add(clienteCuenta);
                _context.SaveChanges();
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
