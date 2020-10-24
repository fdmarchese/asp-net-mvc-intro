using Microsoft.EntityFrameworkCore;
using ejemplo_cta_cte.Models;

namespace ejemplo_cta_cte.Database
{
    public class CtaCteDbContext : DbContext
    {
        public CtaCteDbContext(DbContextOptions<CtaCteDbContext> options) : base(options)
        {
        }

        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<ClienteCuenta> ClienteCuentas { get; set; }
    }
}