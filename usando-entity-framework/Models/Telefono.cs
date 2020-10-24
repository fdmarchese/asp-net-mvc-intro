using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace usando_entity_framework.Models
{
    public class Telefono
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        [RegularExpression(@"[0-9/-]*", ErrorMessage = "El campo {0} sólo admite números y guiones")]
        public string Numero { get; set; }

        [ForeignKey(nameof(TipoTelefono))]
        public Guid TipoTelefonoId { get; set; }
        public TipoTelefono TipoTelefono { get; set; }


        [ForeignKey(nameof(Alumno))]
        public Guid AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
    }
}
