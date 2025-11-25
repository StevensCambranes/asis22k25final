using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Capa_Vista_Seguridad;
namespace Capa_Vista_Comercial
{
    public partial class Frm_Modulos : Form
    {
        public Frm_Modulos()
        {
            InitializeComponent();
        }

        private void btnComercial_Click(object sender, EventArgs e)
        {
            Frm_LoginMDI Logincomercial = new Frm_LoginMDI();
            Logincomercial.Show();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSeguridad_Click(object sender, EventArgs e)
        {
            Frm_Login seguridad = new Frm_Login();
            seguridad.Show();
        }
    }
}
