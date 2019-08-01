using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using HomotorDepotMgrUpdate.Utility;
using System.Threading;

namespace HomotorDepotMgrUpdate
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            //Application.Run(new Form1());
            Console.WriteLine("应用程序开始更新");
            Form1 frm = new Form1();
            frm.UpdateHandler();
            Console.WriteLine("更新结束，正在启动应用程序");
            Thread.Sleep(1000);
        }



    }
}