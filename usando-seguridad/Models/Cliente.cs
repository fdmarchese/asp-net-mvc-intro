using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace usando_seguridad.Models
{
    public class Cliente : Usuario
    {
        public List<ClienteCuenta> Cuentas { get; set; }

        public override Rol Rol => Rol.Cliente;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(8, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Dni { get; set; }

        public string Descripcion => $"{Nombre}, {Apellido} - {Dni}";
    }
}
