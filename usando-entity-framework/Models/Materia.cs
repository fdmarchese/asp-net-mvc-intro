using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace usando_entity_framework.Models
{
    public class Materia
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        [MinLength(2, ErrorMessage = "{0} debe tener un mínimo de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Cuatrimestre { get; set; }

        public List<MateriaAlumno> Alumnos { get; set; }

        [ForeignKey(nameof(Profesor))]
        public Guid ProfesorId { get; set; }
        public Profesor Profesor { get; set; }
    }
}
