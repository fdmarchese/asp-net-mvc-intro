using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace usando_entity_framework.Models
{
    public class Profesor : Usuario
    {
        public List<Materia> Materias { get; set; }
    }
}
