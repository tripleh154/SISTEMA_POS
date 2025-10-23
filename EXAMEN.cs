using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PantallaDeLogin
{
    public partial class EXAMEN : Form
    {
        string cadenaConexion = ConfigurationManager.ConnectionStrings["MiconexionBD"].ConnectionString;
        public EXAMEN()
        {
            InitializeComponent();
        }

        private void Inicio_Sesion()
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                string query = "SELECT COUNT(*) FROM catUsuarios WHERE NombreUsuario = @catUsuario AND Pass = @contraseña";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@catUsuario", TBnombre.Text);
                    comando.Parameters.AddWithValue("@contraseña", TBcontrasena.Text);
                    int count = (int)comando.ExecuteScalar();

                    if (count > 0)
                    {
                        Menu objMenu = new Menu();
                        this.Visible = false;
                        objMenu.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos");
                    }
                }
            }
        }
        private void BTinicio_Click(object sender, EventArgs e)
        {
            Inicio_Sesion();
        }
    }
}
