using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AlphaMobileControls;
using System.Reflection;
using HomotorDepotMgr.Utility;
using HomotorDepotMgr.Model;
using D300.System;

namespace HomotorDepotMgr
{
    public partial class UserLogin : AlphaForm
    {
        public delegate void LoginSelection(int selection, string loginID);
        public event LoginSelection LoginSelectionDelegate;

        string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        Hook hkUserLogin = new Hook("hkUserLogin");
        SQLiteDBHelper db = new SQLiteDBHelper();
        MsgDialog msgDialog = new MsgDialog();
        private int inputFocusIndex = 0;

        public UserLogin()
        {
            InitializeComponent();
            pbxUname.Image = AlphaImage.CreateFromFile(path + @"\Resources\login_people.png");
            pbxUname.Alpha = 200;
            pbxPwd.Image = AlphaImage.CreateFromFile(path + @"\Resources\login_lock.png");
            pbxPwd.Alpha = 200;
            hkUserLogin.KeyHandlerDelegate += new Hook.KeyHandler(hkUserLogin_KeyHandlerDelegate);
            hkUserLogin.Start();
            LoadLoginInfo();
        }

        int hkUserLogin_KeyHandlerDelegate(int vkCode, string clsName)
        {
            int result = 0;
            if (clsName.Equals("hkUserLogin"))
            {
                switch (vkCode)
                {
                    case VirtualKey.VK_ENTER:
                        LoginHandler();
                        result = -1;
                        break;
                    case VirtualKey.VK_ESCAPE:
                        hkUserLogin.Stop();
                        LoginSelectionDelegate(0, "");
                        this.Close();
                        result = -1;
                        break;
                }
            }
            return result;
        }

        public void LoginHandler()
        {
            if (D300SysUI.CheckNetworkStatus())
            {
                hkUserLogin.Stop();
                if (!string.IsNullOrEmpty(cboUname.Text) && !string.IsNullOrEmpty(txtPwd.Text))
                {
                    msgDialog.ShowMessage("正在登录中，请稍后···", 1);
                    ResultModel rModel = DataUpDownload.GetValidateUser(cboUname.Text, txtPwd.Text);
                    if (rModel.IsSuccess)
                    {
                        LoginSelectionDelegate(1, cboUname.Text);
                        this.Close();
                    }
                    else
                    {
                        msgDialog.ShowMessage("登录失败", 1);
                        hkUserLogin.Start();
                        //LoginSelectionDelegate(0, "");
                    }
                }
                else
                {
                    msgDialog.ShowMessage("登录失败", 1);
                    hkUserLogin.Start();
                    //LoginSelectionDelegate(0, "");
                }
            }
            else
            {
                msgDialog.ShowMessage("网络没有连接", 1);
            }
        }

        private void pbxLook_Click(object sender, EventArgs e)
        {
            if (txtPwd.PasswordChar == '*')
            {
                txtPwd.PasswordChar = '\0';
            }
            else
            {
                txtPwd.PasswordChar = '*';
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginHandler();
        }

        public void LoadLoginInfo()
        {
            try
            {
                string sql = @"select LoginID from LoginInfo order by CreateTime desc";
                DataTable dt = db.ExecuteDataTable(sql, null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> infoList = dt.Select().Select(o => o["LoginID"].ToString()).ToList();
                    cboUname.DataSource = infoList;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnAlphabet_Click(object sender, EventArgs e)
        {
            hkUserLogin.Stop();
            AlphabetComplex letterFrm = new AlphabetComplex();
            letterFrm.GetConfirmLetterDelegate += new AlphabetComplex.GetConfirmLetter(letterFrm_GetConfirmLetterDelegate);
            letterFrm.Show();
        }

        void letterFrm_GetConfirmLetterDelegate(int selection, string letter)
        {
            if (selection == 1)
            {
                if (inputFocusIndex == 0)
                {
                    cboUname.Text = letter;
                    cboUname.Focus();
                    //cboUname.Select(cboUname.Text.Length, 0);
                    cboUname.SelectionStart = cboUname.Text.Length;
                }
                else if (inputFocusIndex == 1)
                {
                    txtPwd.Text = letter;
                    txtPwd.Focus();
                    //txtPwd.Select(txtPwd.Text.Length, 0);
                    txtPwd.SelectionStart = txtPwd.Text.Length;
                }
            }
            hkUserLogin.Start();
        }

        private void cboUname_GotFocus(object sender, EventArgs e)
        {
            inputFocusIndex = 0;
        }

        private void txtPwd_GotFocus(object sender, EventArgs e)
        {
            inputFocusIndex = 1;
        }

    }
}