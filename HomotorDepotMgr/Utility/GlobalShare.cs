using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SCAN.Scanner2D;

namespace HomotorDepotMgr.Utility
{
    public static class GlobalShare
    {
        public static string LoginID;
        public static bool IsContinue = false;

        /// <summary>
        /// 定时器实现连续扫描
        /// </summary>
        private static int timeInterval = 1300;
        private static System.Windows.Forms.Timer  timer = new System.Windows.Forms.Timer();

        public static void ContinuousScan()
        {
            if (IsContinue)
            {
                IsContinue = false;
                timer.Enabled = false;
                timer.Tick -= timer_Tick;
            }
            else
            {
                IsContinue = true;
                timer.Interval = timeInterval;
                timer.Tick += new EventHandler(timer_Tick);
                timer.Enabled = true;
            }
        }

        static void timer_Tick(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(Scanner.Instance().ScanReader));
            thread.IsBackground = true;
            thread.Start();
        }

    }
}
