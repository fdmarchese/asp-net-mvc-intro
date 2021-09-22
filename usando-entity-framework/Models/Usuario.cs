using System;
using System.ComponentModel.DataAnnotations;

namespace usando_entity_framework.Models
{
    public abstract class Usuario
    {
        // Esta forma es la correcta de especificar que Id va a ser la PK
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        [MinLength(2, ErrorMessage = "{0} debe tener un mínimo de {1} caracteres")]
        // las RegularExpression solamente se aplican sobre properties de tipo string!!!
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = "El campo {0} sólo admite caracteres alfabéticos")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        [MinLength(2, ErrorMessage = "{0} debe tener un mínimo de {1} caracteres")]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = "El campo {0} sólo admite caracteres alfabéticos")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaNacimiento { get; set; }
    }
}
