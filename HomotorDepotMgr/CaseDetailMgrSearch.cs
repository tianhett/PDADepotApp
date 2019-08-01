using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HomotorDepotMgr.Utility;

namespace HomotorDepotMgr
{
    public partial class CaseDetailMgrSearch : Form
    {
        public delegate void GetConfirmSearch(int selection, int index);
        public event GetConfirmSearch GetConfirmSearchDelegate;

        Hook hkCaseDetailMgrSearch = new Hook("CaseDetailMgrSearch");
        MsgDialog msg = new MsgDialog();

        public CaseDetailMgrSearch(string text)
        {
            InitializeComponent();
            txtSearchIndex.Text = text;
            txtSearchIndex.SelectAll();
            hkCaseDetailMgrSearch.KeyHandlerDelegate += new Hook.KeyHandler(hkCaseDetailMgrSearch_KeyHandlerDelegate);
            hkCaseDetailMgrSearch.Start();
        }

        int hkCaseDetailMgrSearch_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("CaseDetailMgrSearch"))
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
            try
            {
                hkCaseDetailMgrSearch.Stop();
                int index = Convert.ToInt32(txtSearchIndex.Text);
                GetConfirmSearchDelegate(1, index);
                this.Close();
            }
            catch (Exception ex)
            {
                msg.ShowMessage("请输入数字", 1);
            }
        }

        private void CancelHandler()
        {
            hkCaseDetailMgrSearch.Stop();
            GetConfirmSearchDelegate(0, 0);
            this.Close();
        }

        private void btnSearchCancel_Click(object sender, EventArgs e)
        {
            CancelHandler();
        }

        private void btnSearchOK_Click(object sender, EventArgs e)
        {
            OkHandler();
        }
    }
}