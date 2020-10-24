using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ejemplo_cta_cte.Models
{
    public class Cliente
    {
        [Key]
        public Guid Id { get; set; }
       
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage ="El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        [MaxLength(8, ErrorMessage ="El campo {0} admite un máximo de {1} caracteres")]
        public string Dni { get; set; }

        public DateTime? FechaDeNacimiento { get; set; }

        public List<ClienteCuenta> Cuentas { get; set; }

        public string Descripcion => $"{Nombre}, {Apellido} - {Dni}";
    }
}
