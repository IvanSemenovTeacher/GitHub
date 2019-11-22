using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

using MySql.Data.MySqlClient;

namespace RFIDandmonitoringUsers
{
    public partial class Form1 : Form
    {
        Panel holdpanel;
        public Form1(Panel p)
        {
            InitializeComponent();

            holdpanel = p;
            textBox1.GotFocus += TextBox1_GotFocus;
            textBox1.LostFocus += TextBox1_LostFocus;

            textBox2.GotFocus += TextBox2_GotFocus;
            textBox2.LostFocus += TextBox2_LostFocus;
        }
        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Пользователь";
                textBox1.ForeColor = Color.LightGray;
            }
        }

        private void TextBox1_GotFocus(object sender, EventArgs e)
        {
            if(textBox1.Text=="Пользователь")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void TextBox2_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "Пароль";
                textBox2.ForeColor = Color.LightGray;
            }
        }

        private void TextBox2_GotFocus(object sender, EventArgs e)
        {
            if (textBox2.Text == "Пароль")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) || (textBox1.Text=="Пользователь"))
            {

                MessageBox.Show("Ошибка имени пользователя");
                //допустим что зашли
                

            }
            else
            {
                string pass = textBox2.Text;
                if (pass=="Пароль")
                {
                    pass = "";
                }
                string connect = "Server=localhost;Database=RFIDBD"
                + ";port=3306;User Id="+textBox1.Text+";password="+pass+ ";SSL mode =none; " +
                "Allow Zero DateTime = false;"+
 "Convert Zero Datetime = true; ";

                MySqlConnection con = new MySqlConnection(connect);
                try
                {
                    con.Open();

                    this.Dispose();

                    WorkWindow ww = new WorkWindow(con);
                    ww.TopLevel = false;
                    ww.Parent = holdpanel;
                    ww.Visible = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

               
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            UdpClient client = new UdpClient(4210);
            string message = "1";
            byte[] data = Encoding.UTF8.GetBytes(message);
            int numberOfSentBytes = client.Send(data, data.Length);
            client.Close();
        }
    }
    }
