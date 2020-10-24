using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ejemplo_cta_cte.Models
{
    public class Cuenta
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        [MinLength(2, ErrorMessage = "El campo {0} admite un mínimo de {1} caracteres")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Balance { get; set; }

        public List<ClienteCuenta> Clientes { get; set; }

        [ForeignKey(nameof(Sucursal))]
        public Guid SucursalId { get; set; }
        public Sucursal Sucursal { get; set; }

        [ForeignKey(nameof(Moneda))]
        public Guid MonedaId { get; set; }
        public Moneda Moneda { get; set; }
    }
}
