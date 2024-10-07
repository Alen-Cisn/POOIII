using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteTCP
{
    public partial class Form2 : Form
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private int puertoLocal;

        public Form2(string ip, int port)
        {
            InitializeComponent();
            InitializeConection(ip, port);

            this.FormClosing += new FormClosingEventHandler(Form2_FormClosing);
        }

        private void InitializeConection(string ip, int port)
        {
            try
            {
                tcpClient = new TcpClient(ip, port);
                stream = tcpClient.GetStream();
                puertoLocal = ((System.Net.IPEndPoint)tcpClient.Client.LocalEndPoint).Port;
                label1.Text = "ID: " + puertoLocal;

                Task.Run(() => recibirMensajes());
            }
            catch (Exception ex){ 
                MessageBox.Show("Error: " + ex.Message );
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                EnviarMensaje();
            }
        }

        private void EnviarMensaje()
        {
            try
            {
                string mensaje = textBox1.Text.Trim();
                if (!string.IsNullOrWhiteSpace(mensaje))
                {
                    richTextBox1.AppendText("C>: " + mensaje + Environment.NewLine);

                    byte[] datos = Encoding.ASCII.GetBytes(mensaje + Environment.NewLine);
                    stream.Write(datos, 0, datos.Length);

                    textBox1.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void recibirMensajes()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string mensajeRecibido = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    richTextBox1.Invoke((MethodInvoker)delegate
                    {
                        richTextBox1.AppendText("S>: " + mensajeRecibido + Environment.NewLine);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(puertoLocal.ToString());
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tcpClient != null)
            {
                stream.Close();
                tcpClient.Close();
                Application.Exit();
            }
        }
    }
}
