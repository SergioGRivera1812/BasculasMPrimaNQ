using MySql.Data.MySqlClient;
using SimpleTCP;
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
    public partial class Form1 : Form
    {
        Configuracion c = new Configuracion();
        Conexion cnn = new Conexion();
        public Form1()
        {
            InitializeComponent();
            cnn = new Conexion();
        }
        SimpleTcpClient clientM;
        SimpleTcpClient clientMP;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                c.Configuracion_Load(sender, e);
                string ip1 = c.txtIpMoxa1.Text;
                string ip2 = c.txtIpMoxa2.Text;
                int port1 = Convert.ToInt32(c.txtPortMoxa1.Text);
                int port2 = Convert.ToInt32(c.txtPortMoxa2.Text);

                clientM = new SimpleTcpClient();
                clientM.DataReceived += Client_DataReceivedM;
                clientM.StringEncoder = Encoding.ASCII; // Ajusta el encoding según el protocolo utilizado por el Moxa NPort
                clientM.Connect(ip1, port1); // Ingresa la dirección IP y el puerto del Moxa NPort
                
                
                clientMP = new SimpleTcpClient();
                clientMP.DataReceived += Client_DataReceivedMP;
                clientMP.StringEncoder = Encoding.ASCII; // Ajusta el encoding según el protocolo utilizado por el Moxa NPort
                clientMP.Connect(ip2, port2); // Ingresa la dirección IP y el puerto del Moxa NPort
               


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Client_DataReceivedM(object sender, SimpleTCP.Message e)
        {
            string mensaje = e.MessageString;


            mensaje = mensaje.Replace("(kg)", "");
            mensaje = mensaje.Replace("=", "");
            string cadenaLimpia = mensaje.Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("", "").Replace(" ", "");
            // Eliminar los ceros no significativos antes del punto decimal
            cadenaLimpia = cadenaLimpia.TrimStart('0');

            if (cadenaLimpia.StartsWith("."))

                // Comprobar si solo hay un dígito después del punto decimal
                cadenaLimpia = "0" + cadenaLimpia;  // Agregar un cero antes del punto decimal si es necesario
            else
                cadenaLimpia = cadenaLimpia.TrimStart('.');  // Eliminar el punto decimal inicial si hay más de un dígito después de él

            // Asegurarse de realizar la operación en el hilo de la interfaz de usuario
            Invoke((MethodInvoker)delegate
            {
                Muelles.Text = cadenaLimpia;
                Console.WriteLine(Muelles.Text);

            });

        }
        private void Client_DataReceivedMP(object sender, SimpleTCP.Message e)
        {
            try
            {
                string mensaje = e.MessageString;

                mensaje = mensaje.Replace("(kg)", "").Replace("\0", string.Empty);
                mensaje = mensaje.Replace("=", "");
                string cadenaLimpia = mensaje.Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("", "").Replace(" ", "");
                // Eliminar los ceros no significativos antes del punto decimal
                cadenaLimpia = cadenaLimpia.TrimStart('0');

                if (cadenaLimpia.StartsWith("."))

                    // Comprobar si solo hay un dígito después del punto decimal
                    cadenaLimpia = "0" + cadenaLimpia;  // Agregar un cero antes del punto decimal si es necesario
                else
                    cadenaLimpia = cadenaLimpia.TrimStart('.');  // Eliminar el punto decimal inicial si hay más de un dígito después de él

                // Asegurarse de realizar la operación en el hilo de la interfaz de usuario
                Invoke((MethodInvoker)delegate
                {
                    MateriaP.Text = cadenaLimpia;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Moxa 2 no envia datos"+ex);
            }
        }


        private void checkMuelles_CheckedChanged(object sender, EventArgs e)
        {
            if (checkMuelles.Checked == true)
            {
                TtaraM.Visible = true;
                TbrutoM.Visible = true;
                TnetoM.Visible = true;

                TaraM.Visible = true;
                BrutoM.Visible = true;
                NetoM.Visible = true;

                kg1.Visible = true;
                kg2.Visible = true;
                kg3.Visible = true;

                BtaraMuelles.Visible = true;
            }
            else
            {
                TtaraM.Visible = false;
                TbrutoM.Visible = false;
                TnetoM.Visible = false;

                TaraM.Visible = false;
                BrutoM.Visible = false;
                NetoM.Visible = false;

                kg1.Visible = false;
                kg2.Visible = false;
                kg3.Visible = false;

                BtaraMuelles.Visible = false;

            }
        }

        private void checkMP_CheckedChanged(object sender, EventArgs e)
        {
            if (checkMP.Checked == true)
            {
                TtaraMP.Visible = true;
                TbrutoMP.Visible = true;
                TnetoMP.Visible = true;

                TaraMP.Visible = true;
                BrutoMP.Visible = true;
                NetoMP.Visible = true;

                kg4.Visible = true;
                kg5.Visible = true;
                kg6.Visible = true;

                BtaraMP.Visible = true;
            }
            else
            {
                TtaraMP.Visible = false;
                TbrutoMP.Visible = false;
                TnetoMP.Visible = false;

                TaraMP.Visible = false;
                BrutoMP.Visible = false;
                NetoMP.Visible = false;

                kg4.Visible = false;
                kg5.Visible = false;
                kg6.Visible = false;

                BtaraMP.Visible = false;

            }
        }

        private void historialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Historial h = new Historial();
            h.Show();

        }

        private void BtaraMuelles_Click(object sender, EventArgs e)
        {
            double neto;
            double bruto = Convert.ToDouble(Muelles.Text);

            if (double.Parse(TtaraM.Text) >= bruto)
            {
                MessageBox.Show("Tara Incorrecta");
            }
            else
            {
                if (!string.IsNullOrEmpty(TtaraM.Text))
                {
                    neto = bruto - double.Parse(TtaraM.Text);
                    TnetoM.Text = neto.ToString();
                    TbrutoM.Text = bruto.ToString();
                }
                else
                {
                    MessageBox.Show("Favor de Ingresar Tara");
                }
            }

        }

        private void BtaraMP_Click(object sender, EventArgs e)
        {
            double neto;
            double bruto = Convert.ToDouble(MateriaP.Text);

            if (double.Parse(TtaraMP.Text) >= bruto)
            {
                MessageBox.Show("Tara Incorrecta");
            }
            else
            {
                if (!string.IsNullOrEmpty(TtaraMP.Text))
                {
                    neto = bruto - double.Parse(TtaraMP.Text);
                    TnetoMP.Text = neto.ToString();
                    TbrutoMP.Text = bruto.ToString();
                }
                else
                {
                    MessageBox.Show("Favor de Ingresar Tara");
                }
            }
        }

        private void ImprMuelle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Muelles.Text))
            {
                try
                {
                    if (checkMuelles.Checked == true)
                    {
                        string GuardarEt = "INSERT INTO Etiquetas (Fecha,Hora,Bruto,Tara,Neto,PesoA) values ('" +
                        DateTime.Now.ToString("d") + "','" + DateTime.Now.ToString("t") + "','" + Muelles.Text + "','" + TtaraM.Text + "','" + TnetoM.Text + "','" + Muelles.Text + "');";

                        MySqlCommand comando = new MySqlCommand(GuardarEt, cnn.getConexion());
                        comando.ExecuteNonQuery();


                        PrinterSettings ps = new PrinterSettings();
                        printDocument1.PrinterSettings = ps;
                        printDocument1.PrintPage += ImprimirM1;
                        printDocument1.Print();

                        TtaraM.Text = string.Empty;
                        TbrutoM.Text = string.Empty;
                        TnetoM.Text = string.Empty;

                    }
                    else
                    {
                        string GuardarEt = "INSERT INTO Etiquetas (Fecha,Hora,Bruto,Tara,Neto,PesoA) values ('" +
                        DateTime.Now.ToString("d") + "','" + DateTime.Now.ToString("t") + "','" + Muelles.Text + "','" + TtaraM.Text + "','" + TnetoM.Text + "','" + Muelles.Text + "');";

                        MySqlCommand comando = new MySqlCommand(GuardarEt, cnn.getConexion());
                        comando.ExecuteNonQuery();


                        PrinterSettings ps = new PrinterSettings();
                        printDocument2.PrinterSettings = ps;
                        printDocument2.PrintPage += ImprimirM2;
                        printDocument2.Print();

                        TtaraM.Text = string.Empty;
                        TbrutoM.Text = string.Empty;
                        TnetoM.Text = string.Empty;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar en base de datos  "+ex,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }

        }
        private void ImprimirM2(object sender, PrintPageEventArgs e)
        {
            try
            {
                // CONTENIDO DE ETIQUETA
                e.Graphics.DrawString("Almacen de Materia Prima", new Font("arial narrow", 14, FontStyle.Bold), new SolidBrush(Color.Black), 0, 15);
                e.Graphics.DrawString("Báscula de Muelles", new Font("arial narrow", 10, FontStyle.Bold), new SolidBrush(Color.Black), 0, 40);
                e.Graphics.DrawString("Peso Actual:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 65);
                e.Graphics.DrawString(Muelles.Text, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 100, 65);
                e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 65);
                // e.Graphics.DrawString("Fecha:", new Font("arial narrow", 10, FontStyle.Bold), new SolidBrush(Color.Black), 0, 60);
                e.Graphics.DrawString(DateTime.Now.ToString(), new Font("courier new", 9, FontStyle.Bold), new SolidBrush(Color.Black), 0, 110);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message, "Administrador", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ImprimirM1(object sender, PrintPageEventArgs e)
        {
            try
            {

                // TITULO
                e.Graphics.DrawString("Almacen de Materia Prima", new Font("arial narrow", 14, FontStyle.Bold), new SolidBrush(Color.Black), 0, 15);
                e.Graphics.DrawString("Báscula de Muelles", new Font("arial narrow", 10, FontStyle.Bold), new SolidBrush(Color.Black), 0, 35);
                // BRUTO
                e.Graphics.DrawString("Bruto:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 50);
                e.Graphics.DrawString(Muelles.Text, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 60, 50);
                e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 50);
                // TARA
                e.Graphics.DrawString("Tara:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 65);
                e.Graphics.DrawString(TtaraM.Text, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 60, 65);
                e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 65);
                // NETO
                e.Graphics.DrawString("Neto:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 80);
                e.Graphics.DrawString(TnetoM.Text, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 60, 80);
                e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 80);
                // HORA Y FECHA
                // e.Graphics.DrawString("Fecha:", new Font("arial narrow", 10, FontStyle.Bold), new SolidBrush(Color.Black), 0, 1);
                e.Graphics.DrawString(DateTime.Now.ToString(), new Font("courier new", 9, FontStyle.Bold), new SolidBrush(Color.Black), 0, 110);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message, "Administrador", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImprMP_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MateriaP.Text))
            {
                try
                {
                    if (checkMP.Checked == true)
                    {
                        string GuardarEt = "INSERT INTO Etiquetas (Fecha,Hora,Bruto,Tara,Neto,PesoA) values ('" +
                        DateTime.Now.ToString("d") + "','" + DateTime.Now.ToString("t") + "','" + MateriaP.Text + "','" + TtaraMP.Text + "','" + TnetoMP.Text + "','" + MateriaP.Text + "');";

                        MySqlCommand comando = new MySqlCommand(GuardarEt, cnn.getConexion());
                        comando.ExecuteNonQuery();


                        PrinterSettings ps = new PrinterSettings();
                        printDocument3.PrinterSettings = ps;
                        printDocument3.PrintPage += ImprimirMP1;
                        printDocument3.Print();

                        TtaraMP.Text = string.Empty;
                        TbrutoMP.Text = string.Empty;
                        TnetoMP.Text = string.Empty;

                    }
                    else
                    {
                        string GuardarEt = "INSERT INTO Etiquetas (Fecha,Hora,Bruto,Tara,Neto,PesoA) values ('" +
                       DateTime.Now.ToString("d") + "','" + DateTime.Now.ToString("t") + "','" + MateriaP.Text + "','" + TtaraMP.Text + "','" + TnetoMP.Text + "','" + MateriaP.Text + "');";

                        MySqlCommand comando = new MySqlCommand(GuardarEt, cnn.getConexion());
                        comando.ExecuteNonQuery();
                       

                        PrinterSettings ps = new PrinterSettings();
                        printDocument4.PrinterSettings = ps;
                        printDocument4.PrintPage += ImprimirMP2;
                        printDocument4.Print();

                        TtaraMP.Text = string.Empty;
                        TbrutoMP.Text = string.Empty;
                        TnetoMP.Text = string.Empty;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar en base de datos  " + ex, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void ImprimirMP1(object sender, PrintPageEventArgs e)
        {
            try
            {
                // TITULO
                e.Graphics.DrawString("Almacen de Materia Prima", new Font("arial narrow", 14, FontStyle.Bold), new SolidBrush(Color.Black), 0, 15);
                e.Graphics.DrawString("Báscula de M.P", new Font("arial narrow", 10, FontStyle.Bold), new SolidBrush(Color.Black), 0, 35);
                // BRUTO
                e.Graphics.DrawString("Bruto:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 50);
                e.Graphics.DrawString(MateriaP.Text, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 60, 50);
                e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 50);
                // TARA
                e.Graphics.DrawString("Tara:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 65);
                e.Graphics.DrawString(TtaraMP.Text, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 60, 65);
                e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 65);
                // NETO
                e.Graphics.DrawString("Neto:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 80);
                e.Graphics.DrawString(TnetoMP.Text, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 60, 80);
                e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 80);
                // HORA Y FECHA
                // e.Graphics.DrawString("Fecha:", new Font("arial narrow", 10, FontStyle.Bold), new SolidBrush(Color.Black), 0, 1);
                e.Graphics.DrawString(DateTime.Now.ToString(), new Font("courier new", 9, FontStyle.Bold), new SolidBrush(Color.Black), 0, 110);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message, "Administrador ImpresiónM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void ImprimirMP2(object sender, PrintPageEventArgs e)
        {
            try
            {

                // CONTENIDO DE ETIQUETA
                e.Graphics.DrawString("Almacen de Materia Prima", new Font("arial narrow", 14, FontStyle.Bold), new SolidBrush(Color.Black), 0, 15);
                e.Graphics.DrawString("Báscula de M.P", new Font("arial narrow", 10, FontStyle.Bold), new SolidBrush(Color.Black), 0, 40);
                e.Graphics.DrawString("Peso Actual:", new Font("arial narrow", 12, FontStyle.Bold), new SolidBrush(Color.Black), 0, 65);
                e.Graphics.DrawString(MateriaP.Text, new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 100, 65);
                e.Graphics.DrawString("Kg", new Font("courier new", 12, FontStyle.Bold), new SolidBrush(Color.Black), 170, 65);
                // e.Graphics.DrawString("Fecha:", new Font("arial narrow", 10, FontStyle.Bold), new SolidBrush(Color.Black), 0, 60);
                e.Graphics.DrawString(DateTime.Now.ToString(), new Font("courier new", 9, FontStyle.Bold), new SolidBrush(Color.Black), 0, 110);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message, "Administrador ImpresiónMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configuracion c = new Configuracion();
            c.Show();
        }
    }
}

