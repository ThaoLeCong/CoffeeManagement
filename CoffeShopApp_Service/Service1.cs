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

namespace CoffeShopApp_Service
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Interval = 60000;
            timer.Elapsed += timer_Ticker;
            timer.Enabled = true;
            Utilities.WriteLogError("Test Windown Service");
        }

        private void timer_Ticker(object sender, ElapsedEventArgs e)
        {
            
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            Utilities.WriteLogError("Service was stop");
        }
    }
}
