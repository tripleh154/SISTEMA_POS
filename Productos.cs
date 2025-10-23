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
    public partial class Productos : Form
    {
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["MiconexionBD"].ConnectionString;
        public Productos()
        {
            InitializeComponent();
            CargarProductos(dgvProductos);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (btnGuardar.Text == "GUARDAR")
            {
                GuardarProductos();
            }
            else
            {
                ModificarProductos();
            }
        }

        public void ModificarProductos()
        {

        }
        private void GuardarProductos()
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                DateTime fechaHoraLocal = DateTime.Now;
                string query = "INSERT INTO catProductos VALUES (@nombre,@descripcion,@precio,@stock,@fecha)";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", tbNombre.Text);
                    comando.Parameters.AddWithValue("@descripcion", tbDescripcion.Text);
                    comando.Parameters.AddWithValue("@precio", tbPrecio.Text);
                    comando.Parameters.AddWithValue("@stock", tbStock.Text);
                    comando.Parameters.AddWithValue("@fecha", fechaHoraLocal);
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Guardado correctamente ");
                    limpiarPantalla();
                }
            }
        }
        public void limpiarPantalla()
        {
            tbNombre.Text = "";
            tbDescripcion.Text = "";
            tbPrecio.Text = "";
            tbStock.Text = "";
            tbNombre.Focus();
        }

        public void CargarProductos(DataGridView dgvProductos)
        {
            string consulta = "SELECT ProductoID AS ID, Nombre, Descripcion, Precio, Stock, FechaCreacion FROM catProductos order by Nombre ASC";
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
                    DataTable dtProductos = new DataTable();
                    adaptador.Fill(dtProductos);
                    dgvProductos.DataSource = dtProductos;
                    dgvProductos.AutoResizeColumns();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los Productos: " + ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowIndex = e.RowIndex;
                string IdProducto = dgvProductos.Rows[rowIndex].Cells["ID"].Value.ToString();
                tbNombre.Text = dgvProductos.Rows[rowIndex].Cells["Nombre"].Value.ToString();
                tbDescripcion.Text = dgvProductos.Rows[rowIndex].Cells["Descripcion"].Value.ToString();
                tbPrecio.Text = dgvProductos.Rows[rowIndex].Cells["Precio"].Value.ToString();
                tbStock.Text = dgvProductos.Rows[rowIndex].Cells["Stock"].Value.ToString();
                DataGridViewRow selectedRow = dgvProductos.Rows[rowIndex];
                selectedRow.DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                btnGuardar.Text = "MODIFICAR";
            }
        }
    }
}
