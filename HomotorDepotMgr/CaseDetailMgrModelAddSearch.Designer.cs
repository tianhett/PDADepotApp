namespace HomotorDepotMgr
{
    partial class CaseDetailMgrModelAddSearch
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
            this.btnModelAlphabet = new System.Windows.Forms.Button();
            this.btnSearchModelCancel = new System.Windows.Forms.Button();
            this.btnSearchModelOK = new System.Windows.Forms.Button();
            this.txtSearchModel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnModelAlphabet
            // 
            this.btnModelAlphabet.Location = new System.Drawing.Point(34, 78);
            this.btnModelAlphabet.Name = "btnModelAlphabet";
            this.btnModelAlphabet.Size = new System.Drawing.Size(164, 50);
            this.btnModelAlphabet.TabIndex = 15;
            this.btnModelAlphabet.Text = "字母键盘";
            this.btnModelAlphabet.Click += new System.EventHandler(this.btnModelAlphabet_Click);
            // 
            // btnSearchModelCancel
            // 
            this.btnSearchModelCancel.Location = new System.Drawing.Point(135, 148);
            this.btnSearchModelCancel.Name = "btnSearchModelCancel";
            this.btnSearchModelCancel.Size = new System.Drawing.Size(86, 40);
            this.btnSearchModelCancel.TabIndex = 14;
            this.btnSearchModelCancel.Text = "取消";
            this.btnSearchModelCancel.Click += new System.EventHandler(this.btnSearchModelCancel_Click);
            // 
            // btnSearchModelOK
            // 
            this.btnSearchModelOK.Location = new System.Drawing.Point(18, 148);
            this.btnSearchModelOK.Name = "btnSearchModelOK";
            this.btnSearchModelOK.Size = new System.Drawing.Size(86, 40);
            this.btnSearchModelOK.TabIndex = 13;
            this.btnSearchModelOK.Text = "确定";
            this.btnSearchModelOK.Click += new System.EventHandler(this.btnSearchModelOK_Click);
            // 
            // txtSearchModel
            // 
            this.txtSearchModel.Location = new System.Drawing.Point(18, 45);
            this.txtSearchModel.Name = "txtSearchModel";
            this.txtSearchModel.Size = new System.Drawing.Size(203, 23);
            this.txtSearchModel.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 20);
            this.label1.Text = "请输入配件型号：";
            // 
            // CaseDetailMgrModelAddSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 214);
            this.Controls.Add(this.btnModelAlphabet);
            this.Controls.Add(this.btnSearchModelCancel);
            this.Controls.Add(this.btnSearchModelOK);
            this.Controls.Add(this.txtSearchModel);
            this.Controls.Add(this.label1);
            this.Name = "CaseDetailMgrModelAddSearch";
            this.Text = "添加产品";
            this.GotFocus += new System.EventHandler(this.CaseDetailMgrModelAddSearch_GotFocus);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnModelAlphabet;
        private System.Windows.Forms.Button btnSearchModelCancel;
        private System.Windows.Forms.Button btnSearchModelOK;
        private System.Windows.Forms.TextBox txtSearchModel;
        private System.Windows.Forms.Label label1;
    }
}