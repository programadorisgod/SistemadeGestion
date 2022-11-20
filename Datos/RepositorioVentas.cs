using Entidades;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class RepositorioVentas : ConexionBasedeDatos
    {
        SqlCommand cmd;
        public int ReducirCantidad(int codigoproducto, int cantidad, out string mensaje)
        {
            mensaje = string.Empty;
            int respuesta = 0;
            try
            {
                Conexion.Open();
                cmd = new SqlCommand("ReducirStock", Conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Codigo", codigoproducto);
                cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                respuesta = cmd.ExecuteNonQuery();
                if (respuesta < 1)
                {
                    mensaje = "No se puede redicir la cantidad";
                }
            }
            catch (Exception d)
            {
                mensaje = d.Message;
                respuesta = 0;

            }
            return respuesta;


        }


        public int Cantidadnormal(int codigoproducto, int cantidad, out string mensaje)
        {
            mensaje = string.Empty;
            int respuesta = 0;
            try
            {
                Conexion.Open();
                cmd = new SqlCommand("ReducirStock", Conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Codigo", codigoproducto);
                cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                respuesta = cmd.ExecuteNonQuery();
                if (respuesta < 1)
                {
                    mensaje = "No se puede aumentar la cantidad";
                }
            }
            catch (Exception d)
            {
                mensaje = d.Message;
                respuesta = 0;

            }
            return respuesta;

        }

        public string Insertar(Entidades.Ventas venta, out string mensaje)
        {
            int count;
            mensaje = string.Empty;
            Conexion.Open();
            cmd = new SqlCommand("IngresarVentas", Conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcliente", venta.Idcliente);
            cmd.Parameters.AddWithValue("@Cantidad", venta.CantidadProductos);
            cmd.Parameters.AddWithValue("@Totalventa", venta.TotalVenta);
            count = Convert.ToInt32(cmd.ExecuteScalar());
            try
            {
                foreach (DetalleSalida detalle in venta.ListaDetalleSalida)
                {
                    cmd = new SqlCommand("IngresarDetalleVentas");
                    cmd.Parameters.AddWithValue("@id_venta", count);
                    cmd.Parameters.AddWithValue("@coidgo_producto", detalle.CodigoProducto);
                    cmd.Parameters.AddWithValue("@Subtotal", detalle.SubTotal);
                    cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);

                }

                var result = cmd.ExecuteNonQuery();
                if (result < 1)
                {
                    mensaje = "No se pudo registrar la salida de los productos";
                }
                Conexion.Close();

            }
            catch (Exception ex)
            {

                Conexion.Close();
                return ex.Message;
            }
            return mensaje;
        }
    }
}
