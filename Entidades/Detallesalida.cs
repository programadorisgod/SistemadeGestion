using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{

    public class DetalleSalida
    {
        public int IdSalida { get; set; }
        public int CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public string SubTotal { get; set; }
        public string PrecioVenta { get; set; }
    }
}