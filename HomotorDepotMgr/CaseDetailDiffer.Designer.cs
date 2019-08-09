namespace HomotorDepotMgr
{
    partial class CaseDetailDiffer
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
            this.dgDifferList = new System.Windows.Forms.DataGrid();
            this.SuspendLayout();
            // 
            // dgDifferList
            // 
            this.dgDifferList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgDifferList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDifferList.Location = new System.Drawing.Point(0, 0);
            this.dgDifferList.Name = "dgDifferList";
            this.dgDifferList.Size = new System.Drawing.Size(238, 270);
            this.dgDifferList.TabIndex = 0;
            this.dgDifferList.CurrentCellChanged += new System.EventHandler(this.dgDifferList_CurrentCellChanged);
            this.dgDifferList.GotFocus += new System.EventHandler(this.dgDifferList_GotFocus);
            // 
            // CaseDetailDiffer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 270);
            this.Controls.Add(this.dgDifferList);
            this.Name = "CaseDetailDiffer";
            this.Text = "差异对比";
            this.GotFocus += new System.EventHandler(this.CaseDetailDiffer_GotFocus);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid dgDifferList;
    }
}