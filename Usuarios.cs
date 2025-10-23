using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;

namespace PantallaDeLogin
{
    public partial class Usuarios: Form
    {
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["MiconexionBD"].ConnectionString;
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarPass(tbPass.Text))
            {
                GuardarUsuario();
                CargarUsuarios(dgvUsuarios);
            }
            else
            {
                errorProvider1.SetError(tbPass, "La cotraseña debe tener una longitud de 8 a 20 caracteres\nDebe contener Mayusculas y Minusculas\nDebe contener un simbolo");
            }
        }
        private void GuardarUsuario()
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                string seleccion = cbSexo.Text;
                string sexo = (seleccion == "MASCULINO") ? "M" : "F";
                DateTime fechaSeleccionada = dtpFechaNacimiento.Value;
                string fechaNac = fechaSeleccionada.ToString("yyyy/MM/dd");
                string query = "INSERT INTO catUsuario VALUES (@nombre,@telefono,@nombreusuario,@contraseña,@sexo,@fechanac,@domicilio)";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", TbNombre.Text);
                    comando.Parameters.AddWithValue("@telefono", mtbTelefono.Text);
                    comando.Parameters.AddWithValue("@nombreusuario", TBNombreususario.Text);
                    comando.Parameters.AddWithValue("@contraseña", tbPass.Text);
                    comando.Parameters.AddWithValue("@sexo", sexo);
                    comando.Parameters.AddWithValue("@fechanac", fechaNac);
                    comando.Parameters.AddWithValue("@domicilio", TbDomicilio.Text);
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Guardado correctamente ");
                    limpiarPantalla();
                }
            }
        }
        public Usuarios()
        {
            InitializeComponent();
            cbSexo.SelectedIndex = 0;
            mtbTelefono.Mask = "(000) 000-0000";
            dtpFechaNacimiento.Format = DateTimePickerFormat.Custom;
            dtpFechaNacimiento.CustomFormat = "yyyy/MM/dd";
            CargarUsuarios(dgvUsuarios);
        }
        public void CargarUsuarios(DataGridView dgvUsuarios)
        {
            string consulta = "SELECT idUsuario, Nombre, Telefono, NombreUsuario, Sexo, FechaNacimiento, Domicilio FROM catUsuario order by idUsuario DESC";
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
                    DataTable dtUsuarios = new DataTable();
                    adaptador.Fill(dtUsuarios);
                    dgvUsuarios.DataSource = dtUsuarios;
                    dgvUsuarios.AutoResizeColumns();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los usuarios: " + ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool ValidarPass(string contrasena)
        {
            if (contrasena.Length < 8 || contrasena.Length > 20)
            {
                return false;
            }
            string patron = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+=\\[{\\]};:<>|./?,-]).{8,20}$";
            return Regex.IsMatch(contrasena, patron);
        }
        public void limpiarPantalla()
        {
            TbNombre.Text = "";
            TbDomicilio.Text = "";
            TBNombreususario.Text = "";
            mtbTelefono.Text = "";
            string fechaString = "2000-01-01";
            dtpFechaNacimiento.Value = DateTime.Parse(fechaString);
            tbPass.Text = "";
            cbSexo.SelectedIndex = 0;
            TbNombre.Focus();
        }
        private void btnRecargar_Click(object sender, EventArgs e)
        {
            CargarUsuarios(dgvUsuarios);
        }

    }
}
