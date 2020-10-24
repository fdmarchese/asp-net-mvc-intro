using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace usando_entity_framework.Models
{
    public class TipoTelefono
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        public string Descripcion { get; set; }

        // Todos los teléfonos que tienen un cierto tipo de teléfono.
        public List<Telefono> Telefonos { get; set; }
    }
}
