namespace HomotorDepotMgr
{
    partial class CaseDetailMgrAdd
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
            this.btnAddDel = new System.Windows.Forms.Button();
            this.btnAddCancel = new System.Windows.Forms.Button();
            this.btnAddOk = new System.Windows.Forms.Button();
            this.txtNum = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblsNum = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblModel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblProdName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalNum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAddDel
            // 
            this.btnAddDel.Location = new System.Drawing.Point(161, 215);
            this.btnAddDel.Name = "btnAddDel";
            this.btnAddDel.Size = new System.Drawing.Size(72, 42);
            this.btnAddDel.TabIndex = 35;
            this.btnAddDel.Text = "删除";
            this.btnAddDel.Click += new System.EventHandler(this.btnAddDel_Click);
            // 
            // btnAddCancel
            // 
            this.btnAddCancel.Location = new System.Drawing.Point(83, 215);
            this.btnAddCancel.Name = "btnAddCancel";
            this.btnAddCancel.Size = new System.Drawing.Size(72, 42);
            this.btnAddCancel.TabIndex = 34;
            this.btnAddCancel.Text = "取消";
            this.btnAddCancel.Click += new System.EventHandler(this.btnAddCancel_Click);
            // 
            // btnAddOk
            // 
            this.btnAddOk.Location = new System.Drawing.Point(5, 215);
            this.btnAddOk.Name = "btnAddOk";
            this.btnAddOk.Size = new System.Drawing.Size(72, 42);
            this.btnAddOk.TabIndex = 33;
            this.btnAddOk.Text = "确定";
            this.btnAddOk.Click += new System.EventHandler(this.btnAddOk_Click);
            // 
            // txtNum
            // 
            this.txtNum.Location = new System.Drawing.Point(71, 146);
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(158, 23);
            this.txtNum.TabIndex = 32;
            this.txtNum.TextChanged += new System.EventHandler(this.txtNum_TextChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 148);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 20);
            this.label8.Text = "数量：";
            // 
            // lblsNum
            // 
            this.lblsNum.Location = new System.Drawing.Point(72, 112);
            this.lblsNum.Name = "lblsNum";
            this.lblsNum.Size = new System.Drawing.Size(158, 20);
            this.lblsNum.Text = "label5";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 20);
            this.label6.Text = "应发：";
            // 
            // lblModel
            // 
            this.lblModel.Location = new System.Drawing.Point(71, 60);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(158, 44);
            this.lblModel.Text = "label3";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(7, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 20);
            this.label4.Text = "型号：";
            // 
            // lblProdName
            // 
            this.lblProdName.Location = new System.Drawing.Point(71, 5);
            this.lblProdName.Name = "lblProdName";
            this.lblProdName.Size = new System.Drawing.Size(158, 48);
            this.lblProdName.Text = "label2";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.Text = "名称：";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 20);
            this.label2.Text = "累计数量：";
            // 
            // lblTotalNum
            // 
            this.lblTotalNum.Location = new System.Drawing.Point(83, 181);
            this.lblTotalNum.Name = "lblTotalNum";
            this.lblTotalNum.Size = new System.Drawing.Size(146, 20);
            // 
            // CaseDetailMgrAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 270);
            this.Controls.Add(this.lblTotalNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAddDel);
            this.Controls.Add(this.btnAddCancel);
            this.Controls.Add(this.btnAddOk);
            this.Controls.Add(this.txtNum);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblsNum);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblModel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblProdName);
            this.Controls.Add(this.label1);
            this.Name = "CaseDetailMgrAdd";
            this.Text = "配件明细添加";
            this.Deactivate += new System.EventHandler(this.CaseDetailMgrAdd_Deactivate);
            this.Activated += new System.EventHandler(this.CaseDetailMgrAdd_Activated);
            this.GotFocus += new System.EventHandler(this.CaseDetailMgrAdd_GotFocus);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddDel;
        private System.Windows.Forms.Button btnAddCancel;
        private System.Windows.Forms.Button btnAddOk;
        private System.Windows.Forms.TextBox txtNum;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblsNum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblProdName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalNum;
    }
}