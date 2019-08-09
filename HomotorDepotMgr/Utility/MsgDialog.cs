using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HomotorDepotMgr.Utility
{
    public class MsgDialog
    {
        [DllImport("coredll.dll", EntryPoint = "SetForegroundWindow", CharSet = CharSet.Auto)]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("coredll.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("coredll.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto)]
        internal static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        internal const int WM_CLOSE = 0x10;
        public string MsgId = "提示消息";
        Timer timer = new Timer();

        public void ShowMessage(string msg, int msgType)
        {
            int ms = 1000;
            string msgId = "提示消息";
            StartKiller(ms);
            MsgId = msgId;
            switch (msgType)
            {
                case 0:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                    break;
                case 1:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    break;
                case 2:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    break;
                case 3:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    break;
                case 4:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    break;
            }
        }

        public void ShowMessage(string msg, int ms, int msgType)
        {
            string msgId = "提示消息";
            StartKiller(ms);
            MsgId = msgId;
            switch (msgType)
            {
                case 0:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                    break;
                case 1:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    break;
                case 2:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    break;
                case 3:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    break;
                case 4:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    break;
            }
        }

        public void ShowMessage(string msg, int ms, string msgId, int msgType)
        {
            StartKiller(ms);
            MsgId = msgId;
            switch (msgType)
            {
                case 0:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                    break;
                case 1:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    break;
                case 2:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    break;
                case 3:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    break;
                case 4:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    break;
            }
        }

        public void ShowMessage(string msg, string msgId, int msgType)
        {
            MsgId = msgId;
            switch (msgType)
            {
                case 0:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                    break;
                case 1:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    break;
                case 2:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    break;
                case 3:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    break;
                case 4:
                    MessageBox.Show(msg, msgId, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    break;
            }
        }

        public void HideMessage(string msgId)
        {
            if (string.IsNullOrEmpty(msgId))
            {
                msgId = MsgId;
            }
            IntPtr ptr = FindWindow(null, msgId);
            if (ptr != IntPtr.Zero)
            {
                PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        private void StartKiller(int ms)
        {
            timer.Interval = ms;
            timer.Tick += new EventHandler(Timer_Tick);
           timer.Enabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            KillMessageBox();
            //停止Timer 
           timer.Enabled = false;
        }

        private void KillMessageBox()
        {
            //按照MessageBox的标题，找到MessageBox的窗口 
            IntPtr ptr = FindWindow(null, MsgId);
            if (ptr != IntPtr.Zero)
            {
                //找到则关闭MessageBox窗口 
                PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        #region 其他
        public static void WindowClose(string windowName)
        {
            IntPtr ptr = FindWindow(null, windowName);
            if (ptr != IntPtr.Zero)
            {
                PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void ActivateWindow(string windowName)
        {
            IntPtr ptr = FindWindow(null, windowName);
            if (ptr != IntPtr.Zero)
            {
                SetForegroundWindow(ptr);
            }
        }
        #endregion

    }
}
