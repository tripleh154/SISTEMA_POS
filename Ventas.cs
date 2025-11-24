using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PantallaDeLogin
{
    public partial class Ventas: Form
    {
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["MiconexionBD"].ConnectionString;
        string sNombre;
        public Ventas(string Nombre)
        {
            sNombre = Nombre;
            InitializeComponent();
            tbProducto.Focus();
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            DateTime fechaHoraLocal = DateTime.Now;
            lblFecha.Text = fechaHoraLocal.ToString("d");
            lblHora.Text = fechaHoraLocal.ToString("t");
            lblTrabajador.Text = sNombre;
        }

        private void tbProducto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                string consulta = "SELECT ProductoID, Nombre, Precio FROM catProductos WHERE ProductoID = @IDProd AND Estatus = 'TRUE'";
                using (SqlConnection con = new SqlConnection(cadenaConexion))
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand(consulta, con);
                        cmd.Parameters.AddWithValue("@IDProd", tbProducto.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            dgvProductos.Rows.Add(row["ProductoID"], row["Nombre"], row["Precio"],1, row["Precio"]);
                            tbProducto.Text = "";
                            tbProducto.Focus();
                            tbSubtotal.Text = (Convert.ToDouble(tbSubtotal.Text) + Convert.ToDouble(row["Precio"])).ToString();
                            tbIVA.Text = (Convert.ToDouble(tbSubtotal.Text) * .16).ToString();
                            tbTotal.Text = (Convert.ToDouble(tbSubtotal.Text) + Convert.ToDouble(tbIVA.Text)).ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al consultar la base de datos: " + ex.Message);
                    }
                }
            }
        
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Estas seguro que deseas eliminar los datos de venta?", "Eliminar venta", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                tbProducto.Text = "";
                tbProducto.Focus();
                dgvProductos.Rows.Clear();
                tbSubtotal.Text = "0";
                tbIVA.Text = "0";
                tbTotal.Text = "0";
                tbDescuento.Text = "0";
                tbCambio.Text = "";
                tbPagoCon.Text = "";
            }
        }
        
    }
}
