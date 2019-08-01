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
    public partial class Alphabet : Form
    {
        public delegate void GetConfirmLetter(int selection, string letter);
        public event GetConfirmLetter GetConfirmLetterDelegate;

        Hook hkAlphabet = new Hook("Alphabet");
        private bool isCapital = true;

        public Alphabet()
        {
            InitializeComponent();
            btnCaps.ForeColor = Color.Red;
            hkAlphabet.KeyHandlerDelegate += new Hook.KeyHandler(hkAlphabet_KeyHandlerDelegate);
            hkAlphabet.Start();
        }

        int hkAlphabet_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("Alphabet"))
            {
                if (vkCode == VirtualKey.VK_ESCAPE)
                {
                    CancelHandler();
                    return -1;
                }
            }
            return 0;
        }

        private void CancelHandler()
        {
            hkAlphabet.Stop();
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
            hkAlphabet.Stop();
            GetConfirmLetterDelegate(1, letter);
            this.Close();
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
            hkAlphabet.Stop();
            GetConfirmLetterDelegate(1, letter);
            this.Close();
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
            hkAlphabet.Stop();
            GetConfirmLetterDelegate(1, letter);
            this.Close();
        }


    }
}