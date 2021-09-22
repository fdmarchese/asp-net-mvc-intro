using System.Collections.Generic;

namespace usando_entity_framework.Models
{
    public class Alumno : Usuario
    {
        // relación 1:1, o sea, un contacto se relaciona con un alumno y un alumno se relaciona con un contacto
        public Contacto Contacto { get; set; }

        // propiedad de navegación para obtener todos los teléfonos relacionados al alumno
        public List<Telefono> Telefonos { get; set; }

        // cardinalidad n:n, por eso se usa una entidad relacional intermedia llamada MateriaAlumno
        public List<MateriaAlumno> Materias { get; set; }
    }
}
