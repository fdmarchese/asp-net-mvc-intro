using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace usando_seguridad.Models
{
    public class Banco
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        public List<Sucursal> Sucursales { get; set;}
    }
}
