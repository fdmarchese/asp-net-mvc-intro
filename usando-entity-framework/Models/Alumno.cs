using System.Collections.Generic;

namespace usando_entity_framework.Models
{
    public class Alumno : Usuario
    {
        public Contacto Contacto { get; set; }
        public List<Telefono> Telefonos { get; set; }
        public List<MateriaAlumno> Materias { get; set; }
    }
}
