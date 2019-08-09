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
    public partial class CaseDetailMgrBarcodeAdd : Form
    {
        public delegate void GetCaseDetailMgrBarcodeAdd(int selection);
        public event GetCaseDetailMgrBarcodeAdd GetCaseDetailMgrBarcodeAddDelegate;

        Hook hkCaseDetailMgrBarcodeAdd = new Hook("CaseDetailMgrBarcodeAdd");
        SQLiteDBHelper db = new SQLiteDBHelper();
        MsgDialog msg = new MsgDialog();

        private string caseNumberID = string.Empty;
        private string caseNo = string.Empty;

        public CaseDetailMgrBarcodeAdd(string caseNumberID, string caseNo)
        {
            InitializeComponent();
            this.caseNumberID = caseNumberID;
            this.caseNo = caseNo;
            hkCaseDetailMgrBarcodeAdd.KeyHandlerDelegate += new Hook.KeyHandler(hkCaseDetailMgrBarcodeAdd_KeyHandlerDelegate);
            hkCaseDetailMgrBarcodeAdd.Start();
        }

        private void CaseDetailMgrBarcodeAdd_GotFocus(object sender, EventArgs e)
        {
            //hkCaseDetailMgrBarcodeAdd.Start();
        }

        int hkCaseDetailMgrBarcodeAdd_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("CaseDetailMgrBarcodeAdd"))
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

        private void btnBarcodeAlphabet_Click(object sender, EventArgs e)
        {
            hkCaseDetailMgrBarcodeAdd.Stop();
            AlphabetComplex letterFrm = new AlphabetComplex();
            letterFrm.GetConfirmLetterDelegate += new AlphabetComplex.GetConfirmLetter(letterFrm_GetConfirmLetterDelegate);
            letterFrm.Show();
        }

        void letterFrm_GetConfirmLetterDelegate(int selection, string letter)
        {
            if (selection == 1)
            {
                txtSearchBarcode.Text += letter;
                txtSearchBarcode.Focus();
                txtSearchBarcode.Select(txtSearchBarcode.Text.Length, 0);
            }
            hkCaseDetailMgrBarcodeAdd.Start();
        }

        private void btnSearchBarcodeOK_Click(object sender, EventArgs e)
        {
            OkHandler();
        }

        private void btnSearchBarcodeCancel_Click(object sender, EventArgs e)
        {
            CancelHandler();
        }

        private void OkHandler()
        {
            hkCaseDetailMgrBarcodeAdd.Stop();
            CaseDetailMgrAdd detailAddFrm = new CaseDetailMgrAdd(this.caseNumberID, this.txtSearchBarcode.Text, this.caseNo);
            detailAddFrm.Show();
            this.Close();
        }

        private void CancelHandler()
        {
            hkCaseDetailMgrBarcodeAdd.Stop();
            GetCaseDetailMgrBarcodeAddDelegate(0);
            this.Close();
        }

    }
}