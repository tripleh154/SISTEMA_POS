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
            if (e.KeyChar == 13)
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
                            string productoID = row["ProductoID"].ToString();
                            double precioUnitario = Convert.ToDouble(row["Precio"]);

                            // 1. Verificar si el producto ya existe en el DataGridView
                            bool productoEncontrado = false;
                            foreach (DataGridViewRow dgvRow in dgvProductos.Rows)
                            {
                                // La columna ProductoID está en el índice 0
                                if (dgvRow.Cells[0].Value != null && dgvRow.Cells[0].Value.ToString() == productoID)
                                {
                                    // Producto encontrado, aumentar cantidad y recalcular subtotal de la fila
                                    int cantidadActual = Convert.ToInt32(dgvRow.Cells[3].Value);
                                    int nuevaCantidad = cantidadActual + 1;

                                    dgvRow.Cells[3].Value = nuevaCantidad; // Actualizar Cantidad (Columna 3)
                                    dgvRow.Cells[4].Value = nuevaCantidad * precioUnitario; // Actualizar Importe (Columna 4)

                                    productoEncontrado = true;
                                    break;
                                }
                            }

                            // 2. Si el producto NO existe, agregarlo como una nueva fila
                            if (!productoEncontrado)
                            {
                                dgvProductos.Rows.Add(row["ProductoID"], row["Nombre"], precioUnitario, 1, precioUnitario);
                            }

                            // 3. Recalcular los totales de toda la venta
                            RecalcularTotales();

                            tbProducto.Text = "";
                            tbProducto.Focus();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al consultar la base de datos: " + ex.Message);
                    }
                }
            }
        }

        private void RecalcularTotales()
        {
            double subTotal = 0.0;
            // La columna del Importe de la fila está en el índice 4
            const int IMPORTE_COLUMN_INDEX = 4;

            // Sumar el importe de todas las filas del DataGridView
            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                if (row.Cells[IMPORTE_COLUMN_INDEX].Value != null)
                {
                    subTotal += Convert.ToDouble(row.Cells[IMPORTE_COLUMN_INDEX].Value);
                }
            }

            // Aplicar IVA y calcular Total
            double iva = subTotal * 0.16;
            double total = subTotal + iva;

            // Actualizar los TextBoxes
            tbSubtotal.Text = subTotal.ToString("N2");
            tbIVA.Text = iva.ToString("N2");
            tbTotal.Text = total.ToString("N2");
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
