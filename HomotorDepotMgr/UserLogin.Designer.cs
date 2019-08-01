namespace HomotorDepotMgr
{
    partial class UserLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserLogin));
            this.pbxUname = new AlphaMobileControls.AlphaPictureBox();
            this.pbxPwd = new AlphaMobileControls.AlphaPictureBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.pbxLook = new System.Windows.Forms.PictureBox();
            this.cboUname = new System.Windows.Forms.ComboBox();
            this.btnAlphabet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pbxUname
            // 
            this.pbxUname.Location = new System.Drawing.Point(5, 27);
            this.pbxUname.Name = "pbxUname";
            this.pbxUname.Size = new System.Drawing.Size(32, 32);
            this.pbxUname.TabIndex = 8;
            // 
            // pbxPwd
            // 
            this.pbxPwd.Location = new System.Drawing.Point(5, 65);
            this.pbxPwd.Name = "pbxPwd";
            this.pbxPwd.Size = new System.Drawing.Size(32, 32);
            this.pbxPwd.TabIndex = 7;
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(41, 70);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(165, 23);
            this.txtPwd.TabIndex = 4;
            this.txtPwd.GotFocus += new System.EventHandler(this.txtPwd_GotFocus);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(55, 182);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(110, 36);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "登录";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // pbxLook
            // 
            this.pbxLook.Image = ((System.Drawing.Image)(resources.GetObject("pbxLook.Image")));
            this.pbxLook.Location = new System.Drawing.Point(208, 73);
            this.pbxLook.Name = "pbxLook";
            this.pbxLook.Size = new System.Drawing.Size(25, 16);
            this.pbxLook.Click += new System.EventHandler(this.pbxLook_Click);
            // 
            // cboUname
            // 
            this.cboUname.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboUname.Location = new System.Drawing.Point(41, 32);
            this.cboUname.Name = "cboUname";
            this.cboUname.Size = new System.Drawing.Size(192, 23);
            this.cboUname.TabIndex = 3;
            this.cboUname.GotFocus += new System.EventHandler(this.cboUname_GotFocus);
            // 
            // btnAlphabet
            // 
            this.btnAlphabet.Location = new System.Drawing.Point(55, 126);
            this.btnAlphabet.Name = "btnAlphabet";
            this.btnAlphabet.Size = new System.Drawing.Size(110, 36);
            this.btnAlphabet.TabIndex = 9;
            this.btnAlphabet.Text = "字母键盘";
            this.btnAlphabet.Click += new System.EventHandler(this.btnAlphabet_Click);
            // 
            // UserLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 270);
            this.Controls.Add(this.btnAlphabet);
            this.Controls.Add(this.cboUname);
            this.Controls.Add(this.pbxLook);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.pbxPwd);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.pbxUname);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserLogin";
            this.Text = "登录";
            this.ResumeLayout(false);

        }

        #endregion

        private AlphaMobileControls.AlphaPictureBox pbxUname;
        private AlphaMobileControls.AlphaPictureBox pbxPwd;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.PictureBox pbxLook;
        private System.Windows.Forms.ComboBox cboUname;
        private System.Windows.Forms.Button btnAlphabet;

    }
}