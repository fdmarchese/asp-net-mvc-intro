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
    public class AlumnosController : Controller
    {
        private readonly InstitutoDbContext _context;

        public AlumnosController(InstitutoDbContext context)
        {
            _context = context;
        }

        // GET: Alumnos
        public IActionResult Index()
        {
            // TRAEMOS TODOS LOS ALUMNOS DE LA BD Y LOS ORDENAMOS
            var alumnos = _context.Alumnos
                .Include(alumno => alumno.Contacto)
                .ToList()
                .OrderBy(alumno => alumno.Apellido) // PRIMER CRITERIO DE ORDEN ES ASCENDENTE POR APELLIDO
                .ThenByDescending(alumno => alumno.Nombre); // SEGUNDO CRITERIO DE ORDEN ES DESCENDENTE POR NOMBRE

            return View(alumnos);
        }

        public IActionResult FiltroPorNombre(string nombre)
        {
            var alumnos = _context.Alumnos
                .Where(alumno => alumno.Nombre == nombre && alumno.FechaNacimiento > DateTime.Today.AddYears(-20))
                .ToList();

            return View(alumnos);
        }

        // GET: Alumnos/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // OBTENGO 1 ALUMNO POR ID CON LOS DATOS DE LAS PROPIEDADES DE NAVEGACION
            Alumno alumno = _context.Alumnos
                .Include(alumno => alumno.Telefonos).ThenInclude(telefono => telefono.TipoTelefono)
                .Include(alumno => alumno.Contacto)
                .Include(alumno => alumno.Materias).ThenInclude(materiaAlumno => materiaAlumno.Materia).ThenInclude(materia => materia.Profesor)
                .FirstOrDefault(alumno => alumno.Id == id);

            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // GET: Alumnos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alumnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                // PROPIEDADES AUTOCOMPLETADAS.
                alumno.Id = Guid.NewGuid();

                // AGREGO EL OBJETO AL CONTEXTO DE DATOS.
                _context.Alumnos.Add(alumno);

                // GUARDAR EN LA BASE DE DATOS.
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Alumno alumno)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // PROPIEDADES AUTOCOMPLETADAS.
        //        alumno.Id = Guid.NewGuid();

        //        // AGREGO EL OBJETO AL CONTEXTO DE DATOS.
        //        _context.Alumnos.Add(alumno);

        //        // GUARDAR EN LA BASE DE DATOS.
        //        _context.SaveChanges();

        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(alumno);
        //}


        // GET: Alumnos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos.FindAsync(id);

            if (alumno == null)
            {
                return NotFound();
            }
            return View(alumno);
        }

        // POST: Alumnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Alumno model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // LEVANTO LA INFO DEL ALUMNO SIN MODIFICAR DESDE LA DB
                    Alumno alumno = _context.Alumnos.Find(id);

                    // MAPEO LA INFO EDITABLE DEL ALUMNO
                    alumno.Apellido = model.Apellido;
                    alumno.FechaNacimiento = model.FechaNacimiento;

                    // GUARDAN EN LA BASE DE DATOS
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlumnoExists(model.Id))
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
            return View(model);
        }

        // GET: Alumnos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // POST: Alumnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            // OBTIENE EL ALUMNO A ELIMINAR
            var alumno = await _context.Alumnos.FindAsync(id);

            // LO REMUEVE DEL CONTEXTO
            _context.Alumnos.Remove(alumno);

            // GRABA LOS CAMBIOS
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool AlumnoExists(Guid id)
        {
            return _context.Alumnos.Any(e => e.Id == id);
        }
    }
}
