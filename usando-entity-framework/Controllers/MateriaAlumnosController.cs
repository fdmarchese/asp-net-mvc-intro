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
    public class MateriaAlumnosController : Controller
    {
        private readonly InstitutoDbContext _context;

        public MateriaAlumnosController(InstitutoDbContext context)
        {
            _context = context;
        }

        // GET: MateriaAlumnos
        public async Task<IActionResult> Index()
        {
            var alumnosDbContext = _context.MateriaAlumnos.Include(m => m.Alumno).Include(m => m.Materia);
            return View(await alumnosDbContext.ToListAsync());
        }

        // GET: MateriaAlumnos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaAlumno = await _context.MateriaAlumnos
                .Include(m => m.Alumno)
                .Include(m => m.Materia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materiaAlumno == null)
            {
                return NotFound();
            }

            return View(materiaAlumno);
        }

        // GET: MateriaAlumnos/Create
        public IActionResult Create()
        {
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Apellido");
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "Nombre");
            return View();
        }

        // POST: MateriaAlumnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MateriaId,AlumnoId")] MateriaAlumno materiaAlumno)
        {
            if (ModelState.IsValid)
            {
                materiaAlumno.Id = Guid.NewGuid();
                _context.Add(materiaAlumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Apellido", materiaAlumno.AlumnoId);
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "Nombre", materiaAlumno.MateriaId);
            return View(materiaAlumno);
        }

        // GET: MateriaAlumnos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaAlumno = await _context.MateriaAlumnos.FindAsync(id);
            if (materiaAlumno == null)
            {
                return NotFound();
            }
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Apellido", materiaAlumno.AlumnoId);
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "Nombre", materiaAlumno.MateriaId);
            return View(materiaAlumno);
        }

        // POST: MateriaAlumnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,MateriaId,AlumnoId")] MateriaAlumno materiaAlumno)
        {
            if (id != materiaAlumno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materiaAlumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaAlumnoExists(materiaAlumno.Id))
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
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Apellido", materiaAlumno.AlumnoId);
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "Nombre", materiaAlumno.MateriaId);
            return View(materiaAlumno);
        }

        // GET: MateriaAlumnos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaAlumno = await _context.MateriaAlumnos
                .Include(m => m.Alumno)
                .Include(m => m.Materia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materiaAlumno == null)
            {
                return NotFound();
            }

            return View(materiaAlumno);
        }

        // POST: MateriaAlumnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var materiaAlumno = await _context.MateriaAlumnos.FindAsync(id);
            _context.MateriaAlumnos.Remove(materiaAlumno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MateriaAlumnoExists(Guid id)
        {
            return _context.MateriaAlumnos.Any(e => e.Id == id);
        }
    }
}
