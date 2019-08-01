using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HomotorDepotMgr.Utility
{
    public class AlertBoxHelper
    {
        public Dictionary<string, AlertBox> dic = new Dictionary<string, AlertBox>();
        Timer timer = new Timer();
        public string MsgId = "提示消息";

        public void Show(string text, string caption, int style)
        {
            //只提供定时器使用
            int ms = 1000;
            string msgId = "提示消息";
            MsgId = msgId;
            AlertBox alertBox = new AlertBox(text, caption, style);
            dic.Add(msgId, alertBox);
            alertBox.Show();
            StartKiller(ms);
        }

        public void Show(string text, string caption, int style, int ms, string msgId)
        {
            if (ms == 0 || (ms < 0 && ms != -1))
            {
                ms = 1000;
            }
            if (string.IsNullOrEmpty(msgId))
            {
                msgId = "提示消息";
            }
            MsgId = msgId;
            AlertBox alertBox = new AlertBox(text, caption, style);
            dic.Add(msgId, alertBox);
            alertBox.Show();
            if (ms == -1)
            {
                //不放入定时器，需要手动调用关闭方法
            }
            else
            {
                //放入定时器，自动关闭
                StartKiller(ms);
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
            dic[MsgId].Close();
            dic[MsgId].Dispose();
            dic.Remove(MsgId);
        }

        public void Hide(string msgId)
        {
            dic[msgId].Close();
            dic[msgId].Dispose();
            dic.Remove(msgId);
        }

    }
}
