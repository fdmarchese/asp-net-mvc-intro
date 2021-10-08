using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace usando_entity_framework.Models
{
    public class Contacto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser un email válido")]
        public string Email { get; set; }

        [MaxLength(100, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        public string LinkedIn { get; set; }

        [MaxLength(100, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        public string Twitter { get; set; }

        [MaxLength(100, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        public string Instagram { get; set; }

        [MaxLength(100, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        public string Facebook { get; set; }

        public string UnDatoMas { get; set; }

        [ForeignKey(nameof(Alumno))]
        public Guid AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
    }
}