using Microsoft.EntityFrameworkCore;
using usando_seguridad.Models;

namespace usando_seguridad.Database
{
    public class SeguridadDbContext : DbContext
    {
        #region Constructor

        /// <summary>
        /// Este constructor es el que necesitamos para usar luego en la clase <see cref="Startup"/> en
        /// donde inicializamos el objeto de contexto de base de datos que luego será utilizado por toda nuestra aplicación.
        /// </summary>
        public SeguridadDbContext(DbContextOptions<SeguridadDbContext> opciones) : base(opciones)
        {
        }

        #endregion

        // Las propiedades del contexto de datos que utilizaremos serán de tipo 
        // DbSet<MiClase> para cada una de las entidades que queremos que se persista.
        #region Propiedades

        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<ClienteCuenta> ClienteCuentas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }

        #endregion
    }
}
