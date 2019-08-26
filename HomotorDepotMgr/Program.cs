using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using HomotorDepotMgr.Utility;

namespace HomotorDepotMgr
{
    static class Program
    {
        [DllImport("coredll.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("coredll.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);

        const uint WM_DESTROY = 0x0002;//窗体销毁消息

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            IntPtr hwndBarcode = FindWindow(null, "2dscan");

            if (hwndBarcode != IntPtr.Zero)//判断系统自带的二维程序是否在运行
            {
                SendMessage(hwndBarcode, WM_DESTROY, 0, 0);//向窗口发消息让其退出运行
            }
            try
            {
                if (!MutexHelper.IsApplicationOnRun())
                {
                    Application.Run(new Main());
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("异常错误，请重启设备!");//如果该程序已经运行则返回，避免程序重复运行
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "程序异常");
            }
        }
    }
}