using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace RFIDandmonitoringUsers
{
    public partial class WorkWindow : Form
    {
        public MySqlConnection connect;
        public WorkWindow(MySqlConnection connection)
        {
            InitializeComponent();
            connect = connection;

            refreshtable("select id as Номер,name as Имя,dol as Должность from users");
            getinfousers(1);
        }
        /// <summary>
        /// получаем информацию о выбранном пользователе
        /// в datagridview и заносим ее в labelы
        /// </summary>
        /// <param name="i"></param>
        void getinfousers(int i)
        {
            MySqlCommand com = new MySqlCommand("select * from users where id=" + i.ToString(), connect);
            MySqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                label8.Text = "Фамилия \t" + reader.GetValue(2).ToString();
                label13.Text = "Имя \t" + reader.GetValue(1).ToString();
                label14.Text = "Отчество \t" + reader.GetValue(3).ToString();
                label9.Text = "Должность \t" + reader.GetValue(4).ToString();
                if (reader.GetValue(7).ToString() == "0")
                    label11.Text = "Текущий статус \t за территорией";
                else
                    label11.Text = "Текущий статус \t на территории";
            }
            reader.Close();

            com.CommandText = "select fulldate from user_time where id_user=" + i.ToString() + " order by fulldate desc limit 1";
            try
            {
                string fulldate = com.ExecuteScalar().ToString();
                label7.Text = "Последний вход \t" + fulldate;
                string[] partdate = fulldate.Split(' ');
                if (partdate[0] == DateTime.Now.ToShortDateString())
                {
                    string[] psd = partdate[0].Split('.');
                    com.CommandText = "select count(id) from user_time where id_user= " + i.ToString() +
                            " and d_year=" + psd[2] + " and d_month=" + psd[1] + " and d_day=" + psd[0] + ";";
                    label10.Text = "Авторизаций за день \t" + com.ExecuteScalar().ToString();
                }
            }
            catch
            {
                label7.Text = "Последний вход \t - нет авторизаций";
                label11.Text = "Текущий статус \t за территорией";
                label10.Text = "Авторизаций за день \t - нет авторизаций";
            }

        }
        /// <summary>
        /// обновляем datagridview после добавления студентов
        /// </summary>
        /// <param name="sql"></param>
        void refreshtable(string sql)
        {
            MySqlCommand com = new MySqlCommand(sql, connect);
            DataTable table = new DataTable();
            table.Load(com.ExecuteReader());
            dataGridView1.DataSource = table;
            dataGridView2.DataSource = table;
        }

        private void WorkWindow_Load(object sender, EventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }
        int count = 0;
        private void Button3_Click(object sender, EventArgs e)
        {
            string sql = "insert into users(name,sname,fname,dol,rfid,foto) values" +
                 "('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text +
                 "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "');";
            MySqlCommand com = new MySqlCommand(sql, connect);
            com.ExecuteNonQuery();
            count++;
            refreshtable("select id,name,dol from users");
            string number = count.ToString();
            if (number[number.Length - 1] == '1')
                label12.Text = "Добавлена " + number + " запись";

            if ((number[number.Length - 1] == '2') || (number[number.Length - 1] == '3') ||
                (number[number.Length - 1] == '4'))
                label12.Text = "Добавлено " + number + " записи";

            if ((number[number.Length - 1] == '5') || (number[number.Length - 1] == '6') ||
                (number[number.Length - 1] == '7') || (number[number.Length - 1] == '8') ||
                (number[number.Length - 1] == '9') || ((number[number.Length - 1] == '0') && count > 0))
                label12.Text = "Добавлено " + number + " записей";
        }

        private void Button5_Click(object sender, EventArgs e)
        {

            if (dataGridView2.SelectedCells.Count > 0)
            {
                int ind = dataGridView2.SelectedCells[0].RowIndex;
                DataGridViewRow dgvr = dataGridView2.Rows[ind];
                //getinfousers(Convert.ToInt32(dgvr.Cells[0].Value));
                userchart uc = new userchart(connect, Convert.ToInt32(dgvr.Cells[0].Value));
                uc.Show();
            }

            
        }

        private void DataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells.Count > 0)
            {
                int ind = dataGridView2.SelectedCells[0].RowIndex;
                DataGridViewRow dgvr = dataGridView2.Rows[ind];
                getinfousers(Convert.ToInt32(dgvr.Cells[0].Value));
            }
        }
    }
}
