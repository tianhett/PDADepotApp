using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;
using D300.System;
using HomotorDepotMgr.Utility;

namespace HomotorDepotMgr
{
    public partial class CaseMgrAdd : Form
    {
        public delegate void GetCaseNumber(int selection, string num, int type, string oldNumber);
        public event GetCaseNumber GetCaseNumberDelegate;

        MsgDialog msg = new MsgDialog();
        Hook hkCaseMgrAdd = new Hook("CaseMgrAdd");
        private int type = 0;
        private string oldNumber = string.Empty;

        public CaseMgrAdd(string caption, int type, string oldNumber)
        {
            InitializeComponent();
            this.Text = caption;
            this.type = type;
            this.oldNumber = oldNumber;
            if (type == 1)
            {
                txtCaseNum.Text = oldNumber;
                txtCaseNum.SelectAll();
            }
            hkCaseMgrAdd.KeyHandlerDelegate += new Hook.KeyHandler(hkCaseMgrAdd_KeyHandlerDelegate);
            hkCaseMgrAdd.Start();
        }

        private void CaseMgrAdd_GotFocus(object sender, EventArgs e)
        {
            //hkCaseMgrAdd.Start();
        }

        int hkCaseMgrAdd_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if(clsName.Equals("CaseMgrAdd"))
            {
                if (vkCode == VirtualKey.VK_ENTER)
                {
                    CaseAddOKHandler();
                    return -1;
                }
                if (vkCode == VirtualKey.VK_ESCAPE)
                {
                    CaseAddCancelHandler();
                    return -1;
                }
            }
            return 0;
        }

        public void CaseAddOKHandler()
        {
            hkCaseMgrAdd.Stop();
            GetCaseNumberDelegate(1, txtCaseNum.Text, this.type, this.oldNumber);
            this.Close();
        }

        private void btnCaseAddOK_Click(object sender, EventArgs e)
        {
            CaseAddOKHandler();
        }

        public void CaseAddCancelHandler()
        {
            hkCaseMgrAdd.Stop();
            GetCaseNumberDelegate(0, "0", this.type, this.oldNumber);
            this.Close();
        }

        private void btnCaseAddCancel_Click(object sender, EventArgs e)
        {
            CaseAddCancelHandler();
        }

        private void btnAlphabet_Click(object sender, EventArgs e)
        {
            hkCaseMgrAdd.Stop();
            Alphabet letterFrm = new Alphabet();
            letterFrm.GetConfirmLetterDelegate += new Alphabet.GetConfirmLetter(letterFrm_GetConfirmLetterDelegate);
            letterFrm.Show();
        }

        void letterFrm_GetConfirmLetterDelegate(int selection, string letter)
        {
            if (selection == 1)
            {
                txtCaseNum.Text += letter;
                txtCaseNum.Focus();
                txtCaseNum.Select(txtCaseNum.Text.Length, 0);
            }
            hkCaseMgrAdd.Start();
        }

    }
}