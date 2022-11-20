using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_Ciber_Cafe_GUI
{
    public partial class Ventas : Form
    {
        private static int codigoproducto = 0;

        private static string precioventa = string.Empty;
        Logica.ServicioVentas servicioVentas = new Logica.ServicioVentas();
        Logica.ServicioCliente servicioCliente = new Logica.ServicioCliente();
        public Ventas()
        {
            InitializeComponent();
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            Fecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (Char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (var form = new ListarClientes())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    DocClien.Text = form.Cedulacliente;
                    txtnombreclie.Text = form.NombreCliente;
                }
            }
        }

        private void Btnotro_Click(object sender, EventArgs e)
        {

            if (txtcodigoproducto.Text.Trim() == "")
            {
                MessageBox.Show("Por favor, ingrese el codigo del producto, para poder agregarlo", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (txtDescripcion.Text.Trim() == "")
            {
                MessageBox.Show("Por favor llene la descripcion, para poder agregarlo", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (YaExiste())
            {
                MessageBox.Show("Este producto ya se agregó a carrito de compras", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            string mensaje = string.Empty;
            int realizar = servicioVentas.reducirCantidad(codigoproducto, Convert.ToInt32(txtCantidad.Value.ToString()), out mensaje);
            decimal subtotal = 0;
            decimal precioventa = 0;
            if (realizar > 0)
            {
                subtotal = Convert.ToDecimal(txtCantidad.Value.ToString()) * precioventa;
                GrillaClientes.Rows.Add(new object[]
                {
                    codigoproducto.ToString(),
                    txtDescripcion.Text,
                    txtCantidad.Value.ToString(),
                    precioventa.ToString("0.00"),
                    subtotal.ToString("0.00")
                });
                CalcularTotalPagar();
            }
            else
            {
                MessageBox.Show("Error", "¡Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void RealizarVenta()
        {
            if (DocClien.Text.Trim() == "")
            {
                MessageBox.Show("Por favor debe seleccionar la cedula del cliente para poder agregarlo", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (txtnombreclie.Text.Trim() == "")
            {
                MessageBox.Show("Por favor debe seleccionar el nombre del cliente para poder agregarlo", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            int cantidad = 0;
            List<DetalleSalida> detalleSalidas = new List<DetalleSalida>();
            foreach (DataGridViewRow item in GrillaClientes.Rows)
            {
                detalleSalidas.Add(new DetalleSalida()
                {
                    CodigoProducto = Convert.ToInt32(item.Cells["Codigo"].Value.ToString()),
                    Descripcion = item.Cells["Descripcion"].Value.ToString(),
                    PrecioVenta = item.Cells["Precio"].Value.ToString(),
                    Cantidad = Convert.ToInt32(item.Cells["Cantidads"].Value.ToString()),
                    SubTotal = item.Cells["Precio"].Value.ToString()

                });
                cantidad += Convert.ToInt32(item.Cells["Cantidads"].Value.ToString());
            }

            int id_cliente = servicioVentas.BuscarporID(DocClien.Text.ToString());
            Entidades.Ventas ventas = new Entidades.Ventas()
            {

                Idcliente = id_cliente,
                CantidadProductos = cantidad,
                TotalVenta = Convert.ToInt32(lbltotalF.Text),
                ListaDetalleSalida = detalleSalidas

            };


        }

        private bool YaExiste()
        {
            bool mensaje = false;
            if (GrillaClientes.Rows.Count > 0)
            {
                foreach (DataGridViewRow item in GrillaClientes.Rows)
                {

                    if (item.Cells["Codigo"].Value.ToString() == codigoproducto.ToString())
                    {
                        mensaje = true;
                        break;
                    }
                }
            }
            return mensaje;
        }


        private void btnventa_Click(object sender, EventArgs e)
        {
            RealizarVenta();
        }
        void CalcularTotalPagar()
        {
            decimal total = 0;
            if (GrillaClientes.Rows.Count > 0)
            {
                foreach (DataGridViewRow item in GrillaClientes.Rows)
                {
                    total += Convert.ToDecimal(item.Cells["SubTotal"].Value.ToString());
                }
            }
            lbltotal.Text = total.ToString("0.00");
        }

        private void GrillaClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string mensaje = string.Empty;
            int p = e.RowIndex;
            if (p != -1)
            {
                if (GrillaClientes.Columns[e.ColumnIndex].Name == "eliminar")
                {
                    int codigo = Convert.ToInt32(GrillaClientes.Rows[p].Cells["Codigo"].Value.ToString());
                    int cantidad = Convert.ToInt32(GrillaClientes.Rows[p].Cells["Cantidads"].Value.ToString());
                    int realizar = servicioVentas.CantidadNormal(codigo, cantidad, out mensaje);

                    if (realizar > 0)
                    {
                        GrillaClientes.Rows.RemoveAt(p);
                        CalcularTotalPagar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "¡Atencion!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
    }

}
