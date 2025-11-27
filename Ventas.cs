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
        int iIdUsuario;
        public Ventas(string Nombre, int idUsuario)
        {
            sNombre = Nombre;
            iIdUsuario = idUsuario;
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
                            bool productoEncontrado = false;
                            foreach (DataGridViewRow dgvRow in dgvProductos.Rows)
                            {
                                if (dgvRow.Cells[0].Value != null && dgvRow.Cells[0].Value.ToString() == productoID)
                                {
                                    int cantidadActual = Convert.ToInt32(dgvRow.Cells[3].Value);
                                    int nuevaCantidad = cantidadActual + 1;

                                    dgvRow.Cells[3].Value = nuevaCantidad;
                                    dgvRow.Cells[4].Value = nuevaCantidad * precioUnitario;

                                    productoEncontrado = true;
                                    break;
                                }
                            }
                            if (!productoEncontrado)
                            {
                                dgvProductos.Rows.Add(row["ProductoID"], row["Nombre"], precioUnitario, 1, precioUnitario);
                            }
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

        private void LimpiarVenta()
        {
            tbProducto.Text = "";
            tbProducto.Focus();
            dgvProductos.Rows.Clear();
            tbSubtotal.Text = "0.00";
            tbIVA.Text = "0.00";
            tbTotal.Text = "0.00";
            tbDescuento.Text = "0.00";
            tbCambio.Text = "";
            tbPagoCon.Text = "";
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.Rows.Count == 0)
            {
                MessageBox.Show("No hay productos en la lista para finalizar la venta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(tbTotal.Text) || Convert.ToDecimal(tbTotal.Text) <= 0)
            {
                MessageBox.Show("El total de la venta debe ser mayor a cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!double.TryParse(tbPagoCon.Text, out double pagoCon) || pagoCon < Convert.ToDouble(tbTotal.Text))
            {
                MessageBox.Show("El monto de pago debe ser mayor o igual al total de la venta.", "Error de Pago", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbPagoCon.Focus();
                return;
            }
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    string insertVenta = @"
                INSERT INTO tblVentas (idUsuario, FechaVenta, TotalVenta)
                OUTPUT INSERTED.VentaID
                VALUES (@idUsuario, GETDATE(), @TotalVenta)";

                    SqlCommand cmdVenta = new SqlCommand(insertVenta, con, transaction);
                    cmdVenta.Parameters.AddWithValue("@idUsuario", iIdUsuario);
                    cmdVenta.Parameters.AddWithValue("@TotalVenta", Convert.ToDecimal(tbTotal.Text));

                    int ventaID = (int)cmdVenta.ExecuteScalar(); // Ejecutar y obtener el VentaID
                    string insertDetalle = @"
                INSERT INTO tblDetalleVenta (VentaID, ProductoID, Cantidad, PrecioVenta, Importe)
                VALUES (@VentaID, @ProductoID, @Cantidad, @PrecioVenta, @Importe)";

                    foreach (DataGridViewRow row in dgvProductos.Rows)
                    {
                        if (row.IsNewRow) continue;

                        SqlCommand cmdDetalle = new SqlCommand(insertDetalle, con, transaction);
                        cmdDetalle.Parameters.AddWithValue("@VentaID", ventaID);
                        cmdDetalle.Parameters.AddWithValue("@ProductoID", Convert.ToInt32(row.Cells[0].Value));
                        cmdDetalle.Parameters.AddWithValue("@Cantidad", Convert.ToInt32(row.Cells[3].Value));
                        cmdDetalle.Parameters.AddWithValue("@PrecioVenta", Convert.ToDecimal(row.Cells[2].Value));
                        cmdDetalle.Parameters.AddWithValue("@Importe", Convert.ToDecimal(row.Cells[4].Value));
                        cmdDetalle.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    MessageBox.Show($"Venta {ventaID} finalizada y guardada exitosamente.", "Venta Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarVenta();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error al guardar la venta: " + ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
