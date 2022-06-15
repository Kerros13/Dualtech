using System;

namespace Dualtech.Modelos
{
    public class Orden
    {

        public Int64 OrdenId { get; set; }

        public Int64 ClienteId { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

    }
}
