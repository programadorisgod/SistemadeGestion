using Entidades;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_Ciber_Cafe_GUI
{
    public partial class Entrada : Form
    {
        Logica.ServicioProveedor servicioProveedor = new Logica.ServicioProveedor();
        Logica.ServicioProducto servicioProducto = new Logica.ServicioProducto();
        Logica.ServicioEntrada servicioEntrada = new Logica.ServicioEntrada();
        Entidades.EntradaProducto entrada = new Entidades.EntradaProducto();
        public Entrada()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        void RefreshLista()
        {
            grillaRegistroEntradas.DataSource = servicioEntrada.GetAll();
        }
        void Limpiar()
        {
            comboBoxCedulaProveedor.SelectedItem = null;
            comboBoxCodigoProducto.SelectedItem = null;
            textBoxPrecioCompra.Text = "";
            textBoxCantidad.Text = "";
        }

        void Guardar()
        {
            if (comboBoxCedulaProveedor.SelectedItem == null || comboBoxCodigoProducto.SelectedItem == null || textBoxPrecioCompra.Text == "" || textBoxCantidad.Text == "")
            {
                msgError("Llene todos los campos, por favor");
            }
            else
            {
                labelError.Visible = false;
                entrada.CedulaProveedor = comboBoxCedulaProveedor.SelectedItem.ToString().Split(' ')[0];
                entrada.CodigoProducto = int.Parse(comboBoxCodigoProducto.SelectedItem.ToString().Split(' ')[0]);
                entrada.Cantidad = int.Parse(textBoxCantidad.Text);
                entrada.PrecioCompra = double.Parse(textBoxPrecioCompra.Text);
                var Respuesta = MessageBox.Show("Desea agregar la entrada?", "Responde...", MessageBoxButtons.YesNo);

                if (Respuesta == DialogResult.Yes)
                {
                    var mensaje = servicioEntrada.Guardar(entrada);
                    MessageBox.Show(mensaje);
                    Limpiar();
                    RefreshLista();
                }
            }
        }

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox5_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.Transparent;
        }

        private void pictureBox5_MouseMove_1(object sender, MouseEventArgs e)
        {
            pictureBox5.BackColor = Color.FromArgb(30, 30, 30);
        }

        private void pictureBox6_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox6_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBox6.BackColor = Color.Transparent;
        }

        private void pictureBox6_MouseMove_1(object sender, MouseEventArgs e)
        {
            pictureBox6.BackColor = Color.FromArgb(30, 30, 30);
        }

        private void Entrada_Load(object sender, EventArgs e)
        {
            foreach (var item in servicioProveedor.GetAll())
            {
                comboBoxCedulaProveedor.Items.Add(item.Cedula + " - " + item.Nombre);
            }
            foreach (var item in servicioProducto.GetAll())
            {
                comboBoxCodigoProducto.Items.Add(item.Codigo + " - " + item.Nombre);
            }

            RefreshLista();
        }

        private void msgError(string msg)
        {
            labelError.Text = "    " + msg;
            labelError.Visible = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void Entrada_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void textBoxPrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                textBoxCantidad.Focus();
            }
        }

        private void textBoxPrecioCompra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                textBoxCantidad.Focus();
            }
        }

        private void textBoxCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnGuardar_Click(sender, e);
            }
        }

        private void textBoxCantidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                textBoxPrecioCompra.Focus();
            }
        }
    }
}
