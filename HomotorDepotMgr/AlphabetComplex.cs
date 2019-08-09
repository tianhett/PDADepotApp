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
    public partial class AlphabetComplex : Form
    {
        public delegate void GetConfirmLetter(int selection, string letter);
        public event GetConfirmLetter GetConfirmLetterDelegate;

        Hook hkAlphabetComplex = new Hook("AlphabetComplex");
        private bool showResult = false;
        private bool isCapital = true;
        private string content = string.Empty;
        private int tabIndex = 0;
        private TextBox txtResult;
        private Button btnCertain;

        public AlphabetComplex()
        {
            InitializeComponent();
            InitInputBox();
            tabIndex = tabControl1.SelectedIndex;
            btnCaps.ForeColor = Color.Red;
            hkAlphabetComplex.KeyHandlerDelegate += new Hook.KeyHandler(hkAlphabetComplex_KeyHandlerDelegate);
            hkAlphabetComplex.Start();
        }

        private void AlphabetComplex_GotFocus(object sender, EventArgs e)
        {
            //hkAlphabetComplex.Start();
        }

        private void InitInputBox()
        {
            txtResult = new TextBox();
            txtResult.Dock = System.Windows.Forms.DockStyle.Left;
            txtResult.Location = new System.Drawing.Point(0, 0);
            txtResult.Name = "txtResult";
            txtResult.Size = new System.Drawing.Size(190, 23);
            txtResult.TabIndex = 54;
            txtResult.Visible = true;
            txtResult.TextChanged += new System.EventHandler(txtResult_TextChanged);
            btnCertain = new Button();
            btnCertain.BackColor = System.Drawing.SystemColors.ControlLightLight;
            btnCertain.Dock = System.Windows.Forms.DockStyle.Top;
            btnCertain.Location = new System.Drawing.Point(190, 0);
            btnCertain.Name = "btnCertain";
            btnCertain.Size = new System.Drawing.Size(40, 23);
            btnCertain.TabIndex = 55;
            btnCertain.Visible = true;
            btnCertain.Text = "←┘";
            btnCertain.Click += new System.EventHandler(btnCertain_Click);
        }

        private void txtResult_TextChanged(object sender, EventArgs e)
        {
            content = txtResult.Text;
        }

        private void btnCertain_Click(object sender, EventArgs e)
        {
            OKHandler();
        }

        int hkAlphabetComplex_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("AlphabetComplex"))
            {
                if (vkCode == VirtualKey.VK_ENTER)
                {
                    OKHandler();
                    return -1;
                }
                if (vkCode == VirtualKey.VK_ESCAPE)
                {
                    CancelHandler();
                    return -1;
                }
                if (vkCode == VirtualKey.VK_BACK)
                {
                    ShowResult(true);
                }
                if (vkCode == VirtualKey.VK_F1)
                {
                    ShowResultAuto();
                }
            }
            return 0;
        }

        private void ShowResultAuto()
        {
            if (tabIndex != tabControl1.SelectedIndex)
            {
                tabControl1.TabPages[tabIndex].Controls.Remove(txtResult);
                tabControl1.TabPages[tabIndex].Controls.Remove(btnCertain);
                tabIndex = tabControl1.SelectedIndex;
                showResult = false;
            }
            if (showResult)
            {
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Remove(txtResult);
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Remove(btnCertain);
                showResult = false;
            }
            else
            {
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(txtResult);
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(btnCertain);
                showResult = true;
                txtResult.BringToFront();
                btnCertain.BringToFront();
                txtResult.Text = content;
                txtResult.Focus();
                //使光标在最后 
                txtResult.SelectionStart = txtResult.Text.Length;
            }
        }

        private void ShowResult(bool flag)
        {
            if (tabIndex != tabControl1.SelectedIndex)
            {
                tabControl1.TabPages[tabIndex].Controls.Remove(txtResult);
                tabControl1.TabPages[tabIndex].Controls.Remove(btnCertain);
                tabIndex = tabControl1.SelectedIndex;
                showResult = false;
            }
            if (flag)
            {
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(txtResult);
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(btnCertain);
                showResult = true;
                txtResult.BringToFront();
                btnCertain.BringToFront();
                txtResult.Text = content;
                txtResult.Focus();
                txtResult.SelectionStart = txtResult.Text.Length;
            }
            else
            {
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Remove(txtResult);
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Remove(btnCertain);
                showResult = false;
            }
        }

        /*private bool FindInputBox()
        {
            bool flag = false;
            foreach (Control each in tabControl1.TabPages[tabControl1.SelectedIndex].Controls)
            {
                if (each is TextBox)
                {
                    TextBox tbx = (TextBox)each;
                    if (tbx.Name.Equals("txtResult"))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }
         **/

        private void OKHandler()
        {
            hkAlphabetComplex.Stop();
            GetConfirmLetterDelegate(1, content);
            this.Close();
        }

        private void CancelHandler()
        {
            hkAlphabetComplex.Stop();
            GetConfirmLetterDelegate(0, "");
            this.Close();
        }

        private void btnKeyCode_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string letter = string.Empty;
            if (isCapital)
            {
                letter = btn.Text.ToUpper();
            }
            else
            {
                letter = btn.Text.ToLower();
            }
            content += letter;
            ShowResult(true);
        }

        private void btnCaps_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (isCapital)
            {
                isCapital = false;
                btn.ForeColor = Color.Black;
            }
            else
            {
                isCapital = true;
                btn.ForeColor = Color.Red;
            }
        }

        private void btnKeyCodeNumber_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string letter = btn.Text;
            content += letter;
            ShowResult(true);
        }

        private void btnKeyCodeSymbol_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string letter = btn.Text;
            if (letter.Equals("空格"))
            {
                letter = " ";
            }
            else if (letter.Equals("&&"))
            {
                letter = "&";
            }
            content += letter;
            ShowResult(true);
        }


    }
}