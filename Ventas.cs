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
    public partial class Ventas: Form
    {
        string sNombre;
        public Ventas(string Nombre)
        {
            sNombre = Nombre;
            InitializeComponent();
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            DateTime fechaHoraLocal = DateTime.Now;
            lblFecha.Text = fechaHoraLocal.ToString("d");
            lblHora.Text = fechaHoraLocal.ToString("t");
            lblTrabajador.Text = sNombre;
        }
    }
}
