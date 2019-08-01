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
    public partial class CaseMgrSearch : Form
    {
        public delegate void GetConfirmSearch(int selection, string num);
        public event GetConfirmSearch GetConfirmSearchDelegate;

        Hook hkCaseMgrSearch = new Hook("CaseMgrSearch");
        MsgDialog msg = new MsgDialog();

        public CaseMgrSearch(string text)
        {
            InitializeComponent();
            txtSearchNum.Text = text;
            txtSearchNum.SelectAll();
            hkCaseMgrSearch.KeyHandlerDelegate += new Hook.KeyHandler(hkCaseMgrSearch_KeyHandlerDelegate);
            hkCaseMgrSearch.Start();
        }

        int hkCaseMgrSearch_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("CaseMgrSearch"))
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
            hkCaseMgrSearch.Stop();
            GetConfirmSearchDelegate(1, txtSearchNum.Text);
            this.Close();
        }

        private void CancelHandler()
        {
            hkCaseMgrSearch.Stop();
            GetConfirmSearchDelegate(0, "0");
            this.Close();
        }

        private void btnSearchOK_Click(object sender, EventArgs e)
        {
            OkHandler();
        }

        private void btnSearchCancel_Click(object sender, EventArgs e)
        {
            CancelHandler();
        }

        private void btnAlphabet_Click(object sender, EventArgs e)
        {
            hkCaseMgrSearch.Stop();
            Alphabet letterFrm = new Alphabet();
            letterFrm.GetConfirmLetterDelegate += new Alphabet.GetConfirmLetter(letterFrm_GetConfirmLetterDelegate);
            letterFrm.Show();
        }

        void letterFrm_GetConfirmLetterDelegate(int selection, string letter)
        {
            if (selection == 1)
            {
                txtSearchNum.Text += letter;
                txtSearchNum.Focus();
                txtSearchNum.Select(txtSearchNum.Text.Length, 0);
            }
            hkCaseMgrSearch.Start();
        }

    }
}