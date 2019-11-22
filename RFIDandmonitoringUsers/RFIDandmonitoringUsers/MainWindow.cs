using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RFIDandmonitoringUsers
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            

            Form1 f1 = new Form1(panel1);
            f1.TopLevel = false;
            f1.Parent = panel1;
            f1.Visible = true;

        }
    }
}
