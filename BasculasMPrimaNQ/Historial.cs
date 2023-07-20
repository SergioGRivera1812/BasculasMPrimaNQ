using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasculasMPrimaNQ
{
    public partial class Historial : Form
    {
        private Conexion cnn;
        private BindingSource bs = new BindingSource();


        public Historial()
        {
            InitializeComponent();
            cnn = new Conexion();
        }

        private void Historial_Load(object sender, EventArgs e)
        {
            dataGridMovimientos.DataSource = load();

        }

        private DataTable load()
        {
            DataTable vista = new DataTable();
            string vista_gral = "select*from Etiquetas ";
            using (MySqlCommand cmd = new MySqlCommand(vista_gral, cnn.getConexion()))
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                vista.Load(reader);
            }

            vista.Columns["PesoA"].ColumnName = "Peso Actual";
            bs.DataSource = vista;

            return vista;
        }

        void FilterData()
        {
            try
            {
                if (bs.DataSource != null)
                {
                    bs.Filter = $" Fecha LIKE '%{txtFiltrar.Text}%'";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Filtrando datos...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtFiltrar_KeyUp(object sender, KeyEventArgs e)
        {
            FilterData();
        }

        private void dataGridMovimientos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridMovimientos.Columns[e.ColumnIndex].Name == "Imprimir")
            {
                PrinterSettings ps = new PrinterSettings();
                printDocument1.PrinterSettings = ps;
                printDocument1.PrintPage += Reimpresion;
                printDocument1.Print();
            }
        }

        private void Reimpresion(object sender, PrintPageEventArgs e)
        {
            string Fec = this.dataGridMovimientos.SelectedRows[0].Cells[2].Value.ToString();
            string hora = this.dataGridMovimientos.SelectedRows[0].Cells[3].Value.ToString();
            string act = this.dataGridMovimientos.SelectedRows[0].Cells[7].Value.ToString();
            string tara = this.dataGridMovimientos.SelectedRows[0].Cells[5].Value.ToString();
            string neto = this.dataGridMovimientos.SelectedRows[0].Cells[6].Value.ToString();
           
            // TITULO
            e.Graphics.DrawString("Almacen de Materia Prima", new Font("arial narrow", 14, FontStyle.Bold), new SolidBrush(Color.Black), 0, 15);
            e.Graphics.DrawString("Reimpresión", new Font("arial narrow", 10, FontStyle.Bold), new SolidBrush(Color.Black), 0, 35);
            // BRUTO
            e.Graphics.DrawString("Bruto:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 50);
            e.Graphics.DrawString(act, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 60, 50);
            e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 50);
            // TARA
            e.Graphics.DrawString("Tara:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 65);
            e.Graphics.DrawString(tara, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 60, 65);
            e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 65);
            // NETO
            e.Graphics.DrawString("Neto:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 80);
            e.Graphics.DrawString(neto, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 60, 80);
            e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 80);
            // HORA Y FECHA
            // e.Graphics.DrawString("Fecha:", new Font("arial narrow", 10, FontStyle.Bold), new SolidBrush(Color.Black), 0, 1);
            e.Graphics.DrawString(Fec+" "+hora, new Font("courier new", 9, FontStyle.Bold), new SolidBrush(Color.Black), 0, 110);

        }

    }
}
