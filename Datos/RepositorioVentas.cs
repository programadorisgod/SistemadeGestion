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

    }
}
