using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usando_seguridad.Database;
using usando_seguridad.Models;

namespace usando_seguridad.Controllers
{
    [Authorize(Roles = nameof(Rol.Administrador))]
    public class MonedasController : Controller
    {
        private readonly SeguridadDbContext _context;

        public MonedasController(SeguridadDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Monedas.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneda = await _context.Monedas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moneda == null)
            {
                return NotFound();
            }

            return View(moneda);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Moneda moneda)
        {
            if (ModelState.IsValid)
            {
                moneda.Id = Guid.NewGuid();
                _context.Add(moneda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(moneda);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneda = await _context.Monedas.FindAsync(id);
            if (moneda == null)
            {
                return NotFound();
            }
            return View(moneda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Moneda moneda)
        {
            if (id != moneda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moneda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonedaExists(moneda.Id))
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
            return View(moneda);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneda = await _context.Monedas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moneda == null)
            {
                return NotFound();
            }

            return View(moneda);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var moneda = await _context.Monedas.FindAsync(id);
            _context.Monedas.Remove(moneda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonedaExists(Guid id)
        {
            return _context.Monedas.Any(e => e.Id == id);
        }
    }
}
