using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Reflection;

using SQL_cslib;
using SQL_vblib.SQL_vblib;

namespace TestSQL
{
    public partial class Form1 : Form
    {
        //Here is the once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private SQL_cs_connector cs_con = null;
        private SQL_vb_connector vb_con = null;
        private SQL_odbc_connector odbc_con = null;

        public Form1()
        {
            log.Debug("Application started");
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cs_con == null)
            {
                log.Info("Connecting to DB");
                //cs_con = new SQL_cs_connector("192.168.8.99", "TestDatabase1", "Administrator", "Pa$$word");
                cs_con = new SQL_cs_connector();
                cs_con.SetLogger(log);
                cs_con.Connect(ConfigurationManager.AppSettings["DBconnStr"]);                
                this.dataGridView1.DataSource = cs_con.SelectTable("Table1");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cs_con != null)
            {
                cs_con.Disconnect();
                this.dataGridView1.DataSource = null;
                cs_con = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cs_con != null)
            {
                Random random = new Random();
                cs_con.UpdateTable("Table1", "name", random.Next(100));
                this.dataGridView1.DataSource = cs_con.SelectTable("Table1");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cs_con != null)
            {
                cs_con.EmptyTable("Table1");
                this.dataGridView1.DataSource = cs_con.SelectTable("Table1");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            vb_con = new SQL_vb_connector("192.168.8.99", "TestDatabase2", "Administrator", "Pa$$word");
            this.dataGridView1.DataSource = vb_con.SelectTable("Table1");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (vb_con != null)
            {
                Random random = new Random();
                vb_con.UpdateTable("Table1", "name", random.Next(100));
                this.dataGridView1.DataSource = vb_con.SelectTable("Table1");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (vb_con != null)
            {
                vb_con.EmptyTable("Table1");
                this.dataGridView1.DataSource = vb_con.SelectTable("Table1");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (vb_con != null)
            {
                vb_con.Disconnect();
                this.dataGridView1.DataSource = null;
                vb_con = null;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            /*
             * Need to add ODBC DSN entry in...
             * C:\Windows\System32\odbcad32.exe 
             * C:\Windows\SysWOW64\odbcad32.exe [User/System DSN] // under 64-bit OS  *** THIS ***
             * ODBC Name=DSN
             */
            if (odbc_con == null)
            {
                odbc_con = new SQL_odbc_connector("VirtualBoxVM", "Administrator", "Pa$$word");
                this.dataGridView1.DataSource = odbc_con.SelectTable("Table1");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (odbc_con != null)
            {
                Random random = new Random();
                odbc_con.UpdateTable("Table1", "name", random.Next(100));
                this.dataGridView1.DataSource = odbc_con.SelectTable("Table1");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (odbc_con != null)
            {
                odbc_con.EmptyTable("Table1");
                this.dataGridView1.DataSource = odbc_con.SelectTable("Table1");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (odbc_con != null)
            {
                odbc_con.Disconnect();
                this.dataGridView1.DataSource = null;
                odbc_con = null;
            }
        }
    }
}
