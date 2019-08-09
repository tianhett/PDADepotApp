namespace HomotorDepotMgr
{
    partial class MsgBox
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsgBox));
            this.pctImg = new AlphaMobileControls.AlphaPictureBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnMsgOK = new System.Windows.Forms.Button();
            this.btnMsgCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pctImg
            // 
            this.pctImg.Location = new System.Drawing.Point(5, 24);
            this.pctImg.Name = "pctImg";
            this.pctImg.Size = new System.Drawing.Size(48, 48);
            this.pctImg.TabIndex = 5;
            // 
            // lblMsg
            // 
            this.lblMsg.Location = new System.Drawing.Point(59, 37);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(174, 48);
            this.lblMsg.Text = "label1";
            // 
            // btnMsgOK
            // 
            this.btnMsgOK.Location = new System.Drawing.Point(32, 106);
            this.btnMsgOK.Name = "btnMsgOK";
            this.btnMsgOK.Size = new System.Drawing.Size(72, 20);
            this.btnMsgOK.TabIndex = 2;
            this.btnMsgOK.Text = "确定";
            this.btnMsgOK.Click += new System.EventHandler(this.btnMsgOK_Click);
            // 
            // btnMsgCancel
            // 
            this.btnMsgCancel.Location = new System.Drawing.Point(131, 105);
            this.btnMsgCancel.Name = "btnMsgCancel";
            this.btnMsgCancel.Size = new System.Drawing.Size(72, 20);
            this.btnMsgCancel.TabIndex = 3;
            this.btnMsgCancel.Text = "取消";
            this.btnMsgCancel.Click += new System.EventHandler(this.btnMsgCancel_Click);
            // 
            // MsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 138);
            this.Controls.Add(this.btnMsgCancel);
            this.Controls.Add(this.btnMsgOK);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pctImg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MsgBox";
            this.Text = "MsgBox";
            this.GotFocus += new System.EventHandler(this.MsgBox_GotFocus);
            this.ResumeLayout(false);

        }

        #endregion

        private AlphaMobileControls.AlphaPictureBox pctImg;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnMsgOK;
        private System.Windows.Forms.Button btnMsgCancel;
    }
}