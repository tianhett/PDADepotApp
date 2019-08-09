using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HomotorDepotMgr.Utility;
using AlphaMobileControls;
using System.Reflection;

namespace HomotorDepotMgr
{
    public partial class MsgBox : AlphaForm
    {
        public delegate void ConfirmSelection(int selection);
        public event ConfirmSelection ConfirmSelectionDelegate;

        Hook hkMsgBox = new Hook("MsgBoxHook");
        string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

        public MsgBox(string text, string caption, int style)
        {
            InitializeComponent();
            this.Text = caption;
            lblMsg.Text = text;
            switch (style)
            {
                case 0:
                    pctImg.Image = AlphaImage.CreateFromFile(path + @"\Resources\error.png");
                    break;
                case 1:
                    pctImg.Image = AlphaImage.CreateFromFile(path + @"\Resources\information.png");
                    break;
                case 2:
                    pctImg.Image = AlphaImage.CreateFromFile(path + @"\Resources\question.png");
                    break;
                case 3:
                    pctImg.Image = AlphaImage.CreateFromFile(path + @"\Resources\warning.png");
                    break;
            }
            pctImg.Alpha = 200;
            hkMsgBox.KeyHandlerDelegate += new Hook.KeyHandler(hkMsgBox_KeyHandlerDelegate);
            hkMsgBox.Start();
        }

        private void MsgBox_GotFocus(object sender, EventArgs e)
        {
            //hkMsgBox.Start();
        }

        int hkMsgBox_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("MsgBoxHook"))
            {
                if (vkCode == VirtualKey.VK_ENTER)
                {
                    OkHandler();
                    return -1;
                }
                if (vkCode == VirtualKey.VK_ESCAPE)
                {
                    CancelHandler();
                    return -1;
                }
            }
            return 0;
        }

        private void OkHandler()
        {
            hkMsgBox.Stop();
            ConfirmSelectionDelegate(1);
            this.Close();
        }

        private void CancelHandler()
        {
            hkMsgBox.Stop();
            ConfirmSelectionDelegate(0);
            this.Close();
        }

        private void btnMsgOK_Click(object sender, EventArgs e)
        {
            OkHandler();
        }

        private void btnMsgCancel_Click(object sender, EventArgs e)
        {
            CancelHandler();
        }



    }
}