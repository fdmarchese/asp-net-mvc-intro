using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace usando_seguridad.Models
{
    public class ClienteCuenta
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Cuenta))]
        public Guid CuentaId { get; set; }
        public Cuenta Cuenta { get; set; }

        [ForeignKey(nameof(Cliente))]
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public bool EsTitular { get; set; }
    }
}
