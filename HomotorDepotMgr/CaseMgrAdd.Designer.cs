namespace HomotorDepotMgr
{
    partial class CaseMgrAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseMgrAdd));
            this.label1 = new System.Windows.Forms.Label();
            this.btnCaseAddOK = new System.Windows.Forms.Button();
            this.txtCaseNum = new System.Windows.Forms.TextBox();
            this.btnCaseAddCancel = new System.Windows.Forms.Button();
            this.btnAlphabet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(41, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.Text = "箱号：";
            // 
            // btnCaseAddOK
            // 
            this.btnCaseAddOK.Location = new System.Drawing.Point(43, 145);
            this.btnCaseAddOK.Name = "btnCaseAddOK";
            this.btnCaseAddOK.Size = new System.Drawing.Size(76, 40);
            this.btnCaseAddOK.TabIndex = 2;
            this.btnCaseAddOK.Text = "确定";
            this.btnCaseAddOK.Click += new System.EventHandler(this.btnCaseAddOK_Click);
            // 
            // txtCaseNum
            // 
            this.txtCaseNum.Location = new System.Drawing.Point(41, 68);
            this.txtCaseNum.Name = "txtCaseNum";
            this.txtCaseNum.Size = new System.Drawing.Size(165, 23);
            this.txtCaseNum.TabIndex = 0;
            // 
            // btnCaseAddCancel
            // 
            this.btnCaseAddCancel.Location = new System.Drawing.Point(125, 145);
            this.btnCaseAddCancel.Name = "btnCaseAddCancel";
            this.btnCaseAddCancel.Size = new System.Drawing.Size(76, 40);
            this.btnCaseAddCancel.TabIndex = 4;
            this.btnCaseAddCancel.Text = "取消";
            this.btnCaseAddCancel.Click += new System.EventHandler(this.btnCaseAddCancel_Click);
            // 
            // btnAlphabet
            // 
            this.btnAlphabet.Location = new System.Drawing.Point(41, 97);
            this.btnAlphabet.Name = "btnAlphabet";
            this.btnAlphabet.Size = new System.Drawing.Size(165, 40);
            this.btnAlphabet.TabIndex = 6;
            this.btnAlphabet.Text = "字母键盘";
            this.btnAlphabet.Click += new System.EventHandler(this.btnAlphabet_Click);
            // 
            // CaseMgrAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 214);
            this.Controls.Add(this.btnAlphabet);
            this.Controls.Add(this.btnCaseAddCancel);
            this.Controls.Add(this.txtCaseNum);
            this.Controls.Add(this.btnCaseAddOK);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CaseMgrAdd";
            this.Text = "添加箱号";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCaseAddOK;
        private System.Windows.Forms.TextBox txtCaseNum;
        private System.Windows.Forms.Button btnCaseAddCancel;
        private System.Windows.Forms.Button btnAlphabet;
    }
}