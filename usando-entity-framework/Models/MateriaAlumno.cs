using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace usando_entity_framework.Models
{
    public class MateriaAlumno
    {
        [Key]
        public Guid Id { get; set; }


        [ForeignKey(nameof(Materia))]
        public Guid MateriaId { get; set; }
        public Materia Materia { get; set; }

        [ForeignKey(nameof(Alumno))]
        public Guid AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
    }
}
