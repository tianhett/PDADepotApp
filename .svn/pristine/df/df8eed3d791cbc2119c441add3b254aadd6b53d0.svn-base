using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using AlphaMobileControls;

namespace HomotorDepotMgr
{
    public partial class AlertBox : AlphaForm
    {
        string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

        public AlertBox(string text, string caption, int style)
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
            pctImg.Alpha = 175;
        }

        
    }
}