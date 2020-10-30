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
    [Authorize(Roles = nameof(Rol.Administrador))]
    public class SucursalesController : Controller
    {
        private readonly SeguridadDbContext _context;

        public SucursalesController(SeguridadDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var seguridadDbContext = _context.Sucursales.Include(s => s.Banco);
            return View(await seguridadDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales
                .Include(s => s.Banco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        public IActionResult Create()
        {
            ViewData["BancoId"] = new SelectList(_context.Bancos, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sucursal sucursal)
        {
            if (ModelState.IsValid)
            {
                sucursal.Id = Guid.NewGuid();
                _context.Add(sucursal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BancoId"] = new SelectList(_context.Bancos, "Id", "Nombre", sucursal.BancoId);
            return View(sucursal);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal == null)
            {
                return NotFound();
            }
            ViewData["BancoId"] = new SelectList(_context.Bancos, "Id", "Nombre", sucursal.BancoId);
            return View(sucursal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Sucursal sucursal)
        {
            if (id != sucursal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sucursal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SucursalExists(sucursal.Id))
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
            ViewData["BancoId"] = new SelectList(_context.Bancos, "Id", "Nombre", sucursal.BancoId);
            return View(sucursal);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales
                .Include(s => s.Banco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sucursal = await _context.Sucursales.FindAsync(id);
            _context.Sucursales.Remove(sucursal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SucursalExists(Guid id)
        {
            return _context.Sucursales.Any(e => e.Id == id);
        }
    }
}
