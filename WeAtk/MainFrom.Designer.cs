namespace WeAtk
{
    partial class MainForm
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TopBar_1 = new System.Windows.Forms.TabPage();
            this.logBtn = new System.Windows.Forms.Button();
            this.rootbtn = new System.Windows.Forms.Button();
            this.settingbtn = new System.Windows.Forms.Button();
            this.startbtn = new System.Windows.Forms.Button();
            this.loginbtn = new System.Windows.Forms.Button();
            this.TopBar = new System.Windows.Forms.TabControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fightlogBtn = new System.Windows.Forms.Button();
            this.TopBar_1.SuspendLayout();
            this.TopBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopBar_1
            // 
            this.TopBar_1.Controls.Add(this.fightlogBtn);
            this.TopBar_1.Controls.Add(this.logBtn);
            this.TopBar_1.Controls.Add(this.rootbtn);
            this.TopBar_1.Controls.Add(this.settingbtn);
            this.TopBar_1.Controls.Add(this.startbtn);
            this.TopBar_1.Controls.Add(this.loginbtn);
            this.TopBar_1.Location = new System.Drawing.Point(4, 22);
            this.TopBar_1.Name = "TopBar_1";
            this.TopBar_1.Padding = new System.Windows.Forms.Padding(3);
            this.TopBar_1.Size = new System.Drawing.Size(997, 72);
            this.TopBar_1.TabIndex = 0;
            this.TopBar_1.Text = "主菜单";
            this.TopBar_1.UseVisualStyleBackColor = true;
            // 
            // logBtn
            // 
            this.logBtn.Location = new System.Drawing.Point(270, 7);
            this.logBtn.Name = "logBtn";
            this.logBtn.Size = new System.Drawing.Size(60, 60);
            this.logBtn.TabIndex = 6;
            this.logBtn.Text = "管理日志";
            this.logBtn.UseVisualStyleBackColor = true;
            this.logBtn.Click += new System.EventHandler(this.logBtn_Click);
            // 
            // rootbtn
            // 
            this.rootbtn.Location = new System.Drawing.Point(204, 7);
            this.rootbtn.Name = "rootbtn";
            this.rootbtn.Size = new System.Drawing.Size(60, 60);
            this.rootbtn.TabIndex = 5;
            this.rootbtn.Text = "权限锁定";
            this.rootbtn.UseVisualStyleBackColor = true;
            this.rootbtn.Click += new System.EventHandler(this.rootbtn_Click);
            // 
            // settingbtn
            // 
            this.settingbtn.Location = new System.Drawing.Point(138, 7);
            this.settingbtn.Name = "settingbtn";
            this.settingbtn.Size = new System.Drawing.Size(60, 60);
            this.settingbtn.TabIndex = 2;
            this.settingbtn.Text = "参数设置";
            this.settingbtn.UseVisualStyleBackColor = true;
            this.settingbtn.Click += new System.EventHandler(this.settingbtn_Click);
            // 
            // startbtn
            // 
            this.startbtn.Location = new System.Drawing.Point(72, 7);
            this.startbtn.Name = "startbtn";
            this.startbtn.Size = new System.Drawing.Size(60, 60);
            this.startbtn.TabIndex = 1;
            this.startbtn.Text = "开始游戏";
            this.startbtn.UseVisualStyleBackColor = true;
            this.startbtn.Click += new System.EventHandler(this.startbtn_Click);
            // 
            // loginbtn
            // 
            this.loginbtn.Location = new System.Drawing.Point(6, 7);
            this.loginbtn.Name = "loginbtn";
            this.loginbtn.Size = new System.Drawing.Size(60, 60);
            this.loginbtn.TabIndex = 0;
            this.loginbtn.Text = "主机登录";
            this.loginbtn.UseVisualStyleBackColor = true;
            this.loginbtn.Click += new System.EventHandler(this.loginbtn_Click);
            // 
            // TopBar
            // 
            this.TopBar.Controls.Add(this.TopBar_1);
            this.TopBar.Location = new System.Drawing.Point(1, 0);
            this.TopBar.Name = "TopBar";
            this.TopBar.SelectedIndex = 0;
            this.TopBar.Size = new System.Drawing.Size(1005, 98);
            this.TopBar.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Location = new System.Drawing.Point(12, 104);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 600);
            this.panel1.TabIndex = 1;
            // 
            // fightlogBtn
            // 
            this.fightlogBtn.Location = new System.Drawing.Point(336, 7);
            this.fightlogBtn.Name = "fightlogBtn";
            this.fightlogBtn.Size = new System.Drawing.Size(60, 60);
            this.fightlogBtn.TabIndex = 7;
            this.fightlogBtn.Text = "战斗日志";
            this.fightlogBtn.UseVisualStyleBackColor = true;
            this.fightlogBtn.Click += new System.EventHandler(this.fightlogBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.TopBar);
            this.Name = "MainForm";
            this.Text = "微信";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.TopBar_1.ResumeLayout(false);
            this.TopBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage TopBar_1;
        private System.Windows.Forms.Button rootbtn;
        private System.Windows.Forms.Button settingbtn;
        private System.Windows.Forms.Button startbtn;
        private System.Windows.Forms.Button loginbtn;
        private System.Windows.Forms.TabControl TopBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button logBtn;
        private System.Windows.Forms.Button fightlogBtn;
    }
}

