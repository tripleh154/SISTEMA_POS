using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PantallaDeLogin
{
    public partial class Login: Form
    {
        string cadenaConexion = ConfigurationManager.ConnectionStrings["MiconexionBD"].ConnectionString;
        public Login()
        {
            InitializeComponent();
        }
        private void BTinicio_Click(object sender, EventArgs e)
        {
                Inicio_Sesion();
        }
        private void TBnombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                TBcontrasena.Focus();
            }
        }
        private void TBcontrasena_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Inicio_Sesion();
            }
        }
        private void Inicio_Sesion()
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                string query = "SELECT Nombre FROM catUsuario WHERE NombreUsuario = @catUsuario AND Pass = @contraseña";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    int count = 0;
                    string Nombre = "";
                    comando.Parameters.AddWithValue("@catUsuario", TBnombre.Text);
                    comando.Parameters.AddWithValue("@contraseña", TBcontrasena.Text);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        count++;
                        Nombre = reader.GetString(0);
                    }
                    if (count > 0)
                    {
                        Menu objMenu = new Menu(Nombre);
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

        private void TBnombre_Leave(object sender, EventArgs e)
        {
            if (TBnombre.Text != "Juan")
            {
                TBnombre.Focus();
            }
        }
    }
}
