using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ejemplo_cta_cte.Models
{
    public class Moneda
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Codigo { get; set; }

        public List<Cuenta> Cuentas { get; set; }
    }
}
