using System;

namespace Dualtech.Modelos
{
    public class DetalleOrden
    {
        public Int64 DetalleOrdenid { get; set; }

        public Int64 OrdenId { get; set; }

        public Int64 ProductoId { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Impuestos { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }
    }
}