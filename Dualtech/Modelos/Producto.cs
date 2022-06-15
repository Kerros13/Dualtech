using System;

namespace Dualtech.Modelos
{
    public class Producto
    {
        public Int64 ProductoId { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Precio { get; set; }

        public Int64 Existencia { get; set; }
    }
}
