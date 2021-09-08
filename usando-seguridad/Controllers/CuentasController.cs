using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using usando_seguridad.Database;
using usando_seguridad.Models;

namespace usando_seguridad.Controllers
{
    [Authorize] // Solamente debe estar logueado
    public class CuentasController : Controller
    {
        private readonly SeguridadDbContext _context;

        public CuentasController(SeguridadDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador,Cliente")] // tanto un admin como un cliente pueden acceder
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
                .Include(cuenta => cuenta.Clientes).ThenInclude(clienteCuenta => clienteCuenta.Cliente)
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
            ViewData["Clientes"] = new MultiSelectList(_context.Clientes, nameof(Cliente.Id), nameof(Cliente.Descripcion));
            return View();
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cuenta cuenta, Guid[] clienteIds, Guid titularDeCuenta)
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
                        CuentaId = cuenta.Id,
                        EsTitular = clienteId.Equals(titularDeCuenta)
                    };

                    _context.Add(cliente);
                }

                _context.Add(cuenta);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MonedaId"] = new SelectList(_context.Monedas, "Id", "Codigo", cuenta.MonedaId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Nombre", cuenta.SucursalId);
            ViewData["Clientes"] = new MultiSelectList(_context.Clientes, nameof(Cliente.Id), nameof(Cliente.Descripcion));
            return View(cuenta);
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
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

        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Cuenta cuenta, Guid[] clienteIds, Guid titularDeCuenta)
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
                            CuentaId = cuenta.Id,
                            EsTitular = clienteId.Equals(titularDeCuenta)
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

        #region Acciones de Cliente

        [Authorize(Roles = nameof(Rol.Cliente))] // solamente puede acceder un cliente
        [HttpGet]
        public IActionResult MisCuentas()
        {
            var username = User.Identity.Name;
            var cliente = _context.Clientes
                .Include(cliente => cliente.Cuentas).ThenInclude(clienteCuenta => clienteCuenta.Cuenta).ThenInclude(cuenta => cuenta.Moneda)
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

            if (monto <= 0)
            {
                ViewBag.Error = "El monto debe ser un valor positivo";
                return View(cuenta);
            }

            cuenta.Balance += monto;

            var clienteId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var movimiento = new Movimiento()
            {
                Id = Guid.NewGuid(),
                ClienteId = clienteId,
                CuentaId = cuenta.Id,
                Fecha = DateTime.Now,
                Monto = monto
            };

            _context.Add(movimiento);

            _context.SaveChanges();

            return RedirectToAction(nameof(MisCuentas));
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpGet]
        public IActionResult Extraer(Guid id)
        {
            var cuenta = _context.Cuentas.Find(id);
            return View(cuenta);
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpPost]
        public IActionResult Extraer(Guid id, decimal monto)
        {
            var cuenta = _context.Cuentas.Find(id);

            if(cuenta.Balance < monto)
            {
                ViewBag.Error = "No dispone de fondos";
                return View(cuenta);
            }

            cuenta.Balance -= monto;

            var clienteId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var movimiento = new Movimiento()
            {
                Id = Guid.NewGuid(),
                ClienteId = clienteId,
                CuentaId = cuenta.Id,
                Fecha = DateTime.Now,
                Monto = monto * -1
            };

            _context.Add(movimiento);

            _context.SaveChanges();

            return RedirectToAction(nameof(MisCuentas));
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpGet]
        public IActionResult Movimientos(Guid id)
        {
            var cuenta = _context.Cuentas
                .Include(cuenta => cuenta.Movimientos).ThenInclude(movimiento => movimiento.Cliente)
                .FirstOrDefault(cuenta => cuenta.Id == id);

            return View(cuenta);
        }

        #endregion

        private bool CuentaExists(Guid id)
        {
            return _context.Cuentas.Any(e => e.Id == id);
        }
    }
}
