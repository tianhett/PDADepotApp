﻿namespace HomotorDepotMgr
{
    partial class CaseDetailMgrEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseDetailMgrEdit));
            this.label1 = new System.Windows.Forms.Label();
            this.lblProdName = new System.Windows.Forms.Label();
            this.lblModel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblsNum = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNum = new System.Windows.Forms.TextBox();
            this.btnEditOk = new System.Windows.Forms.Button();
            this.btnEditCancel = new System.Windows.Forms.Button();
            this.btnEditDel = new System.Windows.Forms.Button();
            this.lblTotalNum = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.Text = "名称：";
            // 
            // lblProdName
            // 
            this.lblProdName.Location = new System.Drawing.Point(71, 5);
            this.lblProdName.Name = "lblProdName";
            this.lblProdName.Size = new System.Drawing.Size(158, 48);
            this.lblProdName.Text = "label2";
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
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 148);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 20);
            this.label8.Text = "数量：";
            // 
            // txtNum
            // 
            this.txtNum.Location = new System.Drawing.Point(71, 146);
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(158, 23);
            this.txtNum.TabIndex = 14;
            this.txtNum.TextChanged += new System.EventHandler(this.txtNum_TextChanged);
            // 
            // btnEditOk
            // 
            this.btnEditOk.Location = new System.Drawing.Point(5, 215);
            this.btnEditOk.Name = "btnEditOk";
            this.btnEditOk.Size = new System.Drawing.Size(72, 42);
            this.btnEditOk.TabIndex = 15;
            this.btnEditOk.Text = "确定";
            this.btnEditOk.Click += new System.EventHandler(this.btnEditOk_Click);
            // 
            // btnEditCancel
            // 
            this.btnEditCancel.Location = new System.Drawing.Point(83, 215);
            this.btnEditCancel.Name = "btnEditCancel";
            this.btnEditCancel.Size = new System.Drawing.Size(72, 42);
            this.btnEditCancel.TabIndex = 16;
            this.btnEditCancel.Text = "取消";
            this.btnEditCancel.Click += new System.EventHandler(this.btnEditCancel_Click);
            // 
            // btnEditDel
            // 
            this.btnEditDel.Location = new System.Drawing.Point(161, 215);
            this.btnEditDel.Name = "btnEditDel";
            this.btnEditDel.Size = new System.Drawing.Size(72, 42);
            this.btnEditDel.TabIndex = 24;
            this.btnEditDel.Text = "删除";
            this.btnEditDel.Click += new System.EventHandler(this.btnEditDel_Click);
            // 
            // lblTotalNum
            // 
            this.lblTotalNum.Location = new System.Drawing.Point(83, 181);
            this.lblTotalNum.Name = "lblTotalNum";
            this.lblTotalNum.Size = new System.Drawing.Size(146, 20);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 20);
            this.label2.Text = "累计数量：";
            // 
            // CaseDetailMgrEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 270);
            this.Controls.Add(this.lblTotalNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnEditDel);
            this.Controls.Add(this.btnEditCancel);
            this.Controls.Add(this.btnEditOk);
            this.Controls.Add(this.txtNum);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblsNum);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblModel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblProdName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CaseDetailMgrEdit";
            this.Text = "配件明细编辑";
            this.Deactivate += new System.EventHandler(this.CaseDetailMgrEdit_Deactivate);
            this.Activated += new System.EventHandler(this.CaseDetailMgrEdit_Activated);
            this.GotFocus += new System.EventHandler(this.CaseDetailMgrEdit_GotFocus);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProdName;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblsNum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNum;
        private System.Windows.Forms.Button btnEditOk;
        private System.Windows.Forms.Button btnEditCancel;
        private System.Windows.Forms.Button btnEditDel;
        private System.Windows.Forms.Label lblTotalNum;
        private System.Windows.Forms.Label label2;
    }
}