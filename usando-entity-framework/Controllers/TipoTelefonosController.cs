using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using usando_entity_framework.Database;
using usando_entity_framework.Models;

namespace usando_entity_framework.Controllers
{
    public class TipoTelefonosController : Controller
    {
        private readonly InstitutoDbContext _context;

        public TipoTelefonosController(InstitutoDbContext context)
        {
            _context = context;
        }

        // GET: TipoTelefonos
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoTelefonos.ToListAsync());
        }

        // GET: TipoTelefonos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefono = await _context.TipoTelefonos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }

            return View(tipoTelefono);
        }

        // GET: TipoTelefonos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoTelefonos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion")] TipoTelefono tipoTelefono)
        {
            if (ModelState.IsValid)
            {
                tipoTelefono.Id = Guid.NewGuid();
                _context.Add(tipoTelefono);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoTelefono);
        }

        // GET: TipoTelefonos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefono = await _context.TipoTelefonos.FindAsync(id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }
            return View(tipoTelefono);
        }

        // POST: TipoTelefonos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Descripcion")] TipoTelefono tipoTelefono)
        {
            if (id != tipoTelefono.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoTelefono);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoTelefonoExists(tipoTelefono.Id))
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
            return View(tipoTelefono);
        }

        // GET: TipoTelefonos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefono = await _context.TipoTelefonos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }

            return View(tipoTelefono);
        }

        // POST: TipoTelefonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tipoTelefono = await _context.TipoTelefonos.FindAsync(id);
            _context.TipoTelefonos.Remove(tipoTelefono);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoTelefonoExists(Guid id)
        {
            return _context.TipoTelefonos.Any(e => e.Id == id);
        }
    }
}
