using BasculasMPrimaNQ.Properties;
using System;
using System.Windows.Forms;

namespace BasculasMPrimaNQ
{
    public partial class Configuracion : Form
    {
        public Configuracion()
        {
            InitializeComponent();
        }


        public void Configuracion_Load(object sender, EventArgs e)
        {
            CargarConfiguracionMoxa();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                GuardarMoxa();
                MessageBox.Show("Configuración guardada","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }catch(Exception ex)
            {
                MessageBox.Show("Error al guardar configuración "+ex,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }


        private void GuardarMoxa()
        {
            Settings.Default.Moxa1 = txtIpMoxa1.Text;
            Settings.Default.Moxa2 = txtIpMoxa2.Text;
            Settings.Default.PMoxa1 = txtPortMoxa1.Text;
            Settings.Default.PMoxa2 = txtPortMoxa2.Text;

            Settings.Default.Save();
        }
        private void CargarConfiguracionMoxa()
        {
            txtIpMoxa1.Text = Settings.Default.Moxa1;
            txtIpMoxa2.Text = Settings.Default.Moxa2;
            txtPortMoxa1.Text = Settings.Default.PMoxa1;
            txtPortMoxa2.Text = Settings.Default.PMoxa2;

        }

        private void Configuracion_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Moxa1 = txtIpMoxa1.Text;
            Settings.Default.Moxa2 = txtIpMoxa2.Text;
            Settings.Default.PMoxa1 = txtPortMoxa1.Text;
            Settings.Default.PMoxa2 = txtPortMoxa2.Text;
        }
    }
}
