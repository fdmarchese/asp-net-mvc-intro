using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    public class AdministradoresController : Controller
    {
        private readonly SeguridadDbContext _context;

        public AdministradoresController(SeguridadDbContext context)
        {
            _context = context;
        }

        // GET: Administradores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Administradores.ToListAsync());
        }

        // GET: Administradores/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // GET: Administradores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administradores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Administrador administrador, string pass)
        {
            try
            {
                pass.ValidarPassword();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Administrador.Password), ex.Message);
            }

            if (ModelState.IsValid)
            {
                administrador.Id = Guid.NewGuid();
                administrador.Password = pass.Encriptar();
                _context.Add(administrador);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(administrador);
        }

        // GET: Administradores/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradores.FindAsync(id);
            if (administrador == null)
            {
                return NotFound();
            }
            return View(administrador);
        }

        // POST: Administradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Administrador administrador, string pass)
        {
            try
            {
                pass.ValidarPassword();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Administrador.Password), ex.Message);
            }

            if (id != administrador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var adminDatabase = _context.Administradores.Find(id);

                    adminDatabase.Nombre = administrador.Nombre;
                    adminDatabase.Apellido = administrador.Apellido;
                    adminDatabase.FechaNacimiento = administrador.FechaNacimiento;
                    adminDatabase.Username = administrador.Username;

                    if (!string.IsNullOrWhiteSpace(pass))
                    {
                        adminDatabase.Password = pass.Encriptar();
                    }

                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministradorExists(administrador.Id))
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
            return View(administrador);
        }

        private bool AdministradorExists(Guid id)
        {
            return _context.Administradores.Any(e => e.Id == id);
        }
    }
}
