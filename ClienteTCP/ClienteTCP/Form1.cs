using System.Net.Sockets;

namespace ClienteTCP
{
    public partial class Form1 : Form
    {
        public TcpClient tcpClient {  get; private set; }

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string ipServer = textBox1.Text;
                int port = int.Parse(textBox2.Text);

                Form2 form = new Form2(ipServer, port);
                form.Show();

                this.Hide();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
