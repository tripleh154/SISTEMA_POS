using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PantallaDeLogin
{
    public partial class Menu: Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ventasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Crea el objeto para la nueva ventamna de Ventas
            Ventas objVentas = new Ventas();
            objVentas.ShowDialog();
        }

        private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inventario objInventario = new Inventario();
            objInventario.ShowDialog();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reportes objReportes = new Reportes();
            objReportes.ShowDialog();
        }

        private void ususariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Usuarios objUsuarios = new Usuarios();
            objUsuarios.ShowDialog();
        }
    }
}
