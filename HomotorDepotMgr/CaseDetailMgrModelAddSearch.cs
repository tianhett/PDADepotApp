using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HomotorDepotMgr.Utility;
using System.Data.SQLite;

namespace HomotorDepotMgr
{
    public partial class CaseDetailMgrModelAddSearch : Form
    {
        public delegate void GetCaseDetailMgrModelAddSearch(int selection);
        public event GetCaseDetailMgrModelAddSearch GetCaseDetailMgrModelAddSearchDelegate;

        Hook hkCaseDetailMgrModelAddSearch = new Hook("CaseDetailMgrModelAddSearch");
        SQLiteDBHelper db = new SQLiteDBHelper();
        MsgDialog msg = new MsgDialog();

        private string caseNumberID = string.Empty;
        private string caseNo = string.Empty;

        public CaseDetailMgrModelAddSearch(string caseNumberID, string caseNo)
        {
            InitializeComponent();
            this.caseNumberID = caseNumberID;
            this.caseNo = caseNo;
            hkCaseDetailMgrModelAddSearch.KeyHandlerDelegate += new Hook.KeyHandler(hkCaseDetailMgrModelAddSearch_KeyHandlerDelegate);
            hkCaseDetailMgrModelAddSearch.Start();
        }

        int hkCaseDetailMgrModelAddSearch_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("CaseDetailMgrModelAddSearch"))
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

        private void btnModelAlphabet_Click(object sender, EventArgs e)
        {
            hkCaseDetailMgrModelAddSearch.Stop();
            AlphabetComplex letterFrm = new AlphabetComplex();
            letterFrm.GetConfirmLetterDelegate += new AlphabetComplex.GetConfirmLetter(letterFrm_GetConfirmLetterDelegate);
            letterFrm.Show();
        }

        void letterFrm_GetConfirmLetterDelegate(int selection, string letter)
        {
            if (selection == 1)
            {
                txtSearchModel.Text = letter;
                txtSearchModel.Focus();
                txtSearchModel.Select(txtSearchModel.Text.Length, 0);
            }
            hkCaseDetailMgrModelAddSearch.Start();
        }

        private void btnSearchModelOK_Click(object sender, EventArgs e)
        {
            OkHandler();
        }

        private void btnSearchModelCancel_Click(object sender, EventArgs e)
        {
            CancelHandler();
        }

        private void OkHandler()
        {
            hkCaseDetailMgrModelAddSearch.Stop();
            CaseDetailMgrModelAdd detailAddFrm = new CaseDetailMgrModelAdd(this.caseNumberID, this.caseNo, this.txtSearchModel.Text);
            detailAddFrm.Show();
            this.Close();
        }

        private void CancelHandler()
        {
            hkCaseDetailMgrModelAddSearch.Stop();
            GetCaseDetailMgrModelAddSearchDelegate(0);
            this.Close();
        }

    }
}