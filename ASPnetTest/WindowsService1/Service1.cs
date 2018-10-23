using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TestLibrary;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer1 = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            this.timer1.Interval = 3000;   // every 3 sec
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            this.timer1.Enabled = true;
            Logger.Instance.WriteInfoLog("Test Windows Service Started");        
        }

        protected override void OnStop()
        {
            this.timer1.Enabled = false;
            Logger.Instance.WriteInfoLog("Test Windows Service Stopped");
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            this.timer1.Enabled = false;

            // Real job needs few seconds to complete!
            //System.Threading.Thread.Sleep(5000);
            Logger.Instance.WriteInfoLog("Timer tick and some job has been done!");

            this.timer1.Enabled = true;
        }

        static public bool Foo(ref string str)
        {
            str = ">>> NUnit Test case from Windows Service";
            return true;
        }
    }
}
