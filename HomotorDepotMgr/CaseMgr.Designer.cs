namespace HomotorDepotMgr
{
    partial class CaseMgr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseMgr));
            this.dgCaseList = new System.Windows.Forms.DataGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.lblInvoiceTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dgCaseList
            // 
            this.dgCaseList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgCaseList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCaseList.Location = new System.Drawing.Point(0, 20);
            this.dgCaseList.Name = "dgCaseList";
            this.dgCaseList.Size = new System.Drawing.Size(238, 250);
            this.dgCaseList.TabIndex = 1;
            this.dgCaseList.CurrentCellChanged += new System.EventHandler(this.dgCaseList_CurrentCellChanged);
            this.dgCaseList.GotFocus += new System.EventHandler(this.dgCaseList_GotFocus);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(238, 20);
            // 
            // lblInvoiceTitle
            // 
            this.lblInvoiceTitle.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblInvoiceTitle.ForeColor = System.Drawing.Color.DarkRed;
            this.lblInvoiceTitle.Location = new System.Drawing.Point(0, 0);
            this.lblInvoiceTitle.Name = "lblInvoiceTitle";
            this.lblInvoiceTitle.Size = new System.Drawing.Size(236, 16);
            this.lblInvoiceTitle.Text = "label1";
            // 
            // CaseMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 270);
            this.Controls.Add(this.lblInvoiceTitle);
            this.Controls.Add(this.dgCaseList);
            this.Controls.Add(this.splitter1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CaseMgr";
            this.Text = "箱号管理";
            this.Deactivate += new System.EventHandler(this.CaseMgr_Deactivate);
            this.Activated += new System.EventHandler(this.CaseMgr_Activated);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid dgCaseList;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Label lblInvoiceTitle;

    }
}