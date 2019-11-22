using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDandmonitoringUsers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            }
            else
            {
                //пробуем зайти в mysql
            }
        }
    }
}
