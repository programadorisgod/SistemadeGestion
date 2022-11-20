using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class RepositorioProveedor : ConexionBasedeDatos
    {
        SqlCommand cmd;
        public string AddProvider(Proveedor proveedor)
        {
            Conexion.Open();
            cmd = new SqlCommand("AgregarProveedor", Conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cedula", proveedor.Cedula);
            cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
            cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
            cmd.Parameters.AddWithValue("@RazonSocial", proveedor.RazonSocial);
            try
            {
                var result = cmd.ExecuteNonQuery();
                Conexion.Close();
                return result == 1 ? "se agregó el Proveedor" : "error al agregar el Proveedor";
            }
            catch (Exception)
            {
                Conexion.Close();
                return "error al agregar ";
            }
        }


        public List<Proveedor> GetProviderList()
        {
            List<Proveedor> ProviderList = new List<Proveedor>();
            string ssql = String.Format("SELECT * FROM Proveedores");
            cmd = new SqlCommand(ssql, Conexion);
            Conexion.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ProviderList.Add(new Proveedor(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
            }
            Conexion.Close();

            return ProviderList;
        }


        public string EditProduct(Producto producto)
        {
            Conexion.Open();
            cmd = new SqlCommand("EditarProducto", Conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Codigo", producto.Codigo);
            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            cmd.Parameters.AddWithValue("@PrecioVenta", producto.ValorVenta);
            try
            {
                var result = cmd.ExecuteNonQuery();
                Conexion.Close();
                return result == 1 ? "se edtió el Producto" : "error al editar el Producto";
            }
            catch (Exception)
            {
                Conexion.Close();
                return "error al editar";
            }
        }

        public string DeleteProvider(Proveedor proveedor)
        {

            Conexion.Open();
            cmd = new SqlCommand("EliminarProveedor", Conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cedula", proveedor.Cedula);
            try
            {
                var result = cmd.ExecuteNonQuery();
                Conexion.Close();
                return result == 1 ? "se eliminó el Proveedor" : "error al eliminar el Proveedor";
            }
            catch (Exception)
            {
                Conexion.Close();
                return "error al eliminar";
            }
        }
    }
}
