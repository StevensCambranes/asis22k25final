using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Capa_Vista_Comercial
{
    public partial class Frm_PruebaNavegador : Form
    {
        private Timer timerBotones;
        public Frm_PruebaNavegador()
        {
            InitializeComponent();
            //parametros para navegador
            Capa_Controlador_Navegador.Cls_ConfiguracionDataGridView config = new Capa_Controlador_Navegador.Cls_ConfiguracionDataGridView
            {
                Ancho = 1100,
                Alto = 200,
                PosX = 10,
                PosY = 300,
                ColorFondo = Color.AliceBlue,
                TipoScrollBars = ScrollBars.Both,
                Nombre = "dgv_cxc"
            };

            string[] columnas = {
                    "tbl_perfil",
                    "Pk_Id_Perfil",
                    "Cmp_Puesto_Perfil",
                    "Cmp_Descripcion_Perfil",
                    "Cmp_Estado_Perfil",
                    "Cmp_Tipo_Perfil"
                };

            string[] sEtiquetas = {
                    "Perfil",
                    "Puesto",
                    "Descripcion Perfil",
                    "Estado",
                    "Tipo Perfil"
                };


            int id_aplicacion = 4406;
            int id_Modulo = 9;
            navegador1.IPkId_Aplicacion = id_aplicacion;
            navegador1.IPkId_Modulo = id_Modulo;
            navegador1.configurarDataGridView(config);
            navegador1.SNombreTabla = columnas[0];
            navegador1.SAlias = columnas;
            navegador1.SEtiquetas = sEtiquetas;
            navegador1.mostrarDatos();

            // Activar los botones al inicio
            ActivarBotonesInternos(navegador1);

            // Crear un temporizador que re-activa botones cada 0.5 segundos
            timerBotones = new Timer();
            timerBotones.Interval = 500; // medio segundo
            timerBotones.Tick += (s, e) => ActivarBotonesInternos(navegador1);
            timerBotones.Start();

        }

        // Función recursiva para habilitar botones dentro del navegador
        public void ActivarBotonesInternos(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c is Button btn)
                    btn.Enabled = true;
                else if (c.HasChildren)
                    ActivarBotonesInternos(c);
            }
        }
    }
}