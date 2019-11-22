using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;

namespace RFIDandmonitoringUsers
{
    public partial class userchart : Form
    {
        public MySqlConnection connect;
        int num2;
        int autorize;
        //ChartArea chArea;
        public userchart(MySqlConnection con,int num)
        {
            InitializeComponent();
            connect = con;
            num2 = num;
            //выбираем года
            string sql = "select d_year from user_time where id_user = " + num.ToString() +" group by d_year";
            //string sql = "Select count(id_user) from user_time where id_user="+num.ToString();
            MySqlCommand command = new MySqlCommand(sql, connect);
            //autorize= Convert.ToInt32(command.ExecuteScalar().ToString());
            MySqlDataReader reader = command.ExecuteReader();
            //записываем года в combobox
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetValue(0));
            }
            reader.Close();
            //добавляем новую область графика
            chart1.Series.Add("посещения за год");
            chart1.Series[0].ChartType = SeriesChartType.FastLine;
        }

        void convertmonthfromnumber(int i)
        {
            switch (i)
            {
                case 1: comboBox2.Items.Add("Январь"); break;
                case 2:
                    comboBox2.Items.Add("Февраль"); break;
                case 3:
                    comboBox2.Items.Add("Март"); break;
                case 4:
                    comboBox2.Items.Add("Апрель"); break;
                case 5:
                    comboBox2.Items.Add("Май"); break;
                case 6:
                    comboBox2.Items.Add("Июнь"); break;
                case 7:
                    comboBox2.Items.Add("Июль"); break;
                case 8:
                    comboBox2.Items.Add("Август"); break;
                case 9:
                    comboBox2.Items.Add("Сентябрь"); break;
                case 10:
                    comboBox2.Items.Add("Октябрь"); break;
                case 11:
                    comboBox2.Items.Add("Ноябрь"); break;
                case 12: comboBox2.Items.Add("Декабрь"); break;
                default:break;
                    
            }
        }

        int convertnumberfrommonth(string i)
        {
            if (i == "Январь") return 1;
            if (i == "Февраль") return 2;
            if (i == "Март") return 3;
            if (i == "Апрель") return 4;
            if (i == "Май") return 5;
            if (i == "Июнь") return 6;
            if (i == "Июль") return 7;
            if (i == "Август") return 8;
            if (i == "Сентябрь") return 9;
            if (i == "Октябрь") return 10;
            if (i == "Ноябрь") return 11;
            if (i == "Декабрь") return 12;
           
            return 0;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart1.Series[0].ChartType = SeriesChartType.FastLine;
            comboBox2.Items.Clear();
            chart1.Series[0].Points.Clear();
            MySqlCommand command = new MySqlCommand();
            command.Connection = connect;
            //выбираем месяца, когда был пользователь
            command.CommandText = "select d_month from user_time where  id_user=" + num2.ToString() + 
                " and d_year="+comboBox1.SelectedItem.ToString() +" group by d_month";
            List<int> month = new List<int>();
            //записываем номера месяцов в список
            
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                month.Add(reader.GetInt32(0));
                convertmonthfromnumber(reader.GetInt32(0));
            }
            reader.Close();

            int countmount = month.Count;
            if (countmount!= 0)
            {
                comboBox2.Visible = true;
            }
                
            for (int i = 0; i < countmount; i++)
            {
                //выбираем количество дней за месяц, когда был пользователь
                command.CommandText = "select count(d_day) from user_time where  id_user=" + num2.ToString() +
                    " and d_month=" + month[i];
                chart1.Series[0].Points.Add(Convert.ToInt32(command.ExecuteScalar()));
            }
            
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart1.Series[0].ChartType = SeriesChartType.Column;
            chart1.Series[0].Points.Clear();
            chart1.Series[0].LegendText = "Посещения за " + comboBox2.SelectedItem;

            MySqlCommand command = new MySqlCommand();
            command.Connection = connect;

            command.CommandText = "select d_day, count(d_day) from user_time where id_user=" + num2.ToString() +
                    " and d_month="+ convertnumberfrommonth(comboBox2.SelectedItem.ToString()) + " and d_year="+comboBox1.SelectedItem +" group by d_day";

             MySqlDataReader reader = command.ExecuteReader();
            //добавление значений на график
            int iter = 0;
             while (reader.Read())
             {
                 chart1.Series[0].Points.Add(reader.GetUInt32(1));
                int r = reader.GetInt32(0);
                int c = convertnumberfrommonth(comboBox2.SelectedItem.ToString());
                if ((r<10)&&(c<10)) chart1.Series[0].Points[iter].AxisLabel ="0"+r+"."+ "0" + convertnumberfrommonth(comboBox2.SelectedItem.ToString());
                if ((r < 10) && (c >=10)) chart1.Series[0].Points[iter].AxisLabel = "0" + r + "." + convertnumberfrommonth(comboBox2.SelectedItem.ToString());
                if ((r >= 10) && (c < 10)) chart1.Series[0].Points[iter].AxisLabel = r + "." + "0" + convertnumberfrommonth(comboBox2.SelectedItem.ToString());
                if ((r >= 10) && (c >= 10)) chart1.Series[0].Points[iter].AxisLabel = r + "." + convertnumberfrommonth(comboBox2.SelectedItem.ToString());
                iter++;
            }
             reader.Close();
            //делаем подпись диаграммы
            chart1.ChartAreas[0].AxisX.Title = comboBox2.SelectedItem.ToString();
            // тест
            //делаем подписи для каждого столбца диаграммы
          //  chart1.Series[0].Points[0].AxisLabel = "gthd";
           // chart1.Series[0].Points[1].AxisLabel = "zxc";
        }
    }
}
