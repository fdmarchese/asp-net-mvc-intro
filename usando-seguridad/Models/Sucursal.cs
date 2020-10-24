using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace usando_seguridad.Models
{
    public class Sucursal
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        public string Nombre { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Direccion { get; set; }

        [ForeignKey(nameof(Banco))]
        public Guid BancoId { get; set; }
        public Banco Banco { get; set; }

        public List<Cuenta> Cuentas { get; set; }

        public string NombreYDireccion => $"{Nombre} {Direccion}";
    }
}
