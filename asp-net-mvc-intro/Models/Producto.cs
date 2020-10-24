using System;

namespace asp_net_mvc_intro.Models
{
    public class Producto
    {
        public Producto()
        {

        }

        public Producto(Guid id, string descripcion, int orden, decimal precio)
        {
            Id = id;
            Descripcion = descripcion;
            Orden = orden;
            Precio = precio;
        }

        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public decimal Precio { get; set; }
    }
}
