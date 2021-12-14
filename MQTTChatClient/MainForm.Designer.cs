
namespace MQTTClientFormTest
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.tabControlChat = new System.Windows.Forms.TabControl();
            this.panelCenterTop = new System.Windows.Forms.Panel();
            this.btnJoinChat = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.tbIp = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblIp = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.panelCenterTop.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panelBottom);
            this.panelMain.Controls.Add(this.panelTop);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(800, 450);
            this.panelMain.TabIndex = 0;
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.tabControlChat);
            this.panelCenter.Controls.Add(this.panelCenterTop);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Enabled = false;
            this.panelCenter.Location = new System.Drawing.Point(0, 83);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(800, 330);
            this.panelCenter.TabIndex = 2;
            // 
            // tabControlChat
            // 
            this.tabControlChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlChat.Location = new System.Drawing.Point(0, 36);
            this.tabControlChat.Name = "tabControlChat";
            this.tabControlChat.SelectedIndex = 0;
            this.tabControlChat.Size = new System.Drawing.Size(800, 294);
            this.tabControlChat.TabIndex = 1;
            // 
            // panelCenterTop
            // 
            this.panelCenterTop.Controls.Add(this.btnJoinChat);
            this.panelCenterTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCenterTop.Location = new System.Drawing.Point(0, 0);
            this.panelCenterTop.Name = "panelCenterTop";
            this.panelCenterTop.Size = new System.Drawing.Size(800, 36);
            this.panelCenterTop.TabIndex = 0;
            // 
            // btnJoinChat
            // 
            this.btnJoinChat.Location = new System.Drawing.Point(13, 7);
            this.btnJoinChat.Name = "btnJoinChat";
            this.btnJoinChat.Size = new System.Drawing.Size(75, 23);
            this.btnJoinChat.TabIndex = 0;
            this.btnJoinChat.Text = "加入聊天室";
            this.btnJoinChat.UseVisualStyleBackColor = true;
            this.btnJoinChat.Click += new System.EventHandler(this.btnJoinChat_Click);
            // 
            // panelBottom
            // 
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 413);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(800, 37);
            this.panelBottom.TabIndex = 1;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.tbPassword);
            this.panelTop.Controls.Add(this.tbUserName);
            this.panelTop.Controls.Add(this.lblPassword);
            this.panelTop.Controls.Add(this.lblUserName);
            this.panelTop.Controls.Add(this.btnDisconnect);
            this.panelTop.Controls.Add(this.btnConnect);
            this.panelTop.Controls.Add(this.nudPort);
            this.panelTop.Controls.Add(this.tbIp);
            this.panelTop.Controls.Add(this.lblPort);
            this.panelTop.Controls.Add(this.lblIp);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 83);
            this.panelTop.TabIndex = 0;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(256, 49);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(118, 23);
            this.tbPassword.TabIndex = 5;
            this.tbPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbIp_KeyPress);
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(51, 49);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(150, 23);
            this.tbUserName.TabIndex = 4;
            this.tbUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbIp_KeyPress);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(218, 52);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(31, 15);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "密碼";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(13, 52);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(31, 15);
            this.lblUserName.TabIndex = 6;
            this.lblUserName.Text = "帳號";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(488, 9);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 9;
            this.btnDisconnect.Text = "登出";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(398, 9);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "連線";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(254, 10);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(120, 23);
            this.nudPort.TabIndex = 3;
            this.nudPort.Value = new decimal(new int[] {
            1883,
            0,
            0,
            0});
            this.nudPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbIp_KeyPress);
            // 
            // tbIp
            // 
            this.tbIp.Location = new System.Drawing.Point(51, 10);
            this.tbIp.MaxLength = 50;
            this.tbIp.Name = "tbIp";
            this.tbIp.PlaceholderText = "請輸入Server IP";
            this.tbIp.Size = new System.Drawing.Size(150, 23);
            this.tbIp.TabIndex = 2;
            this.tbIp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbIp_KeyPress);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(218, 14);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(30, 15);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port";
            // 
            // lblIp
            // 
            this.lblIp.AutoSize = true;
            this.lblIp.Location = new System.Drawing.Point(27, 14);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new System.Drawing.Size(17, 15);
            this.lblIp.TabIndex = 0;
            this.lblIp.Text = "IP";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelMain);
            this.Name = "MainForm";
            this.Text = "MQTT聊天室";
            this.panelMain.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.panelCenterTop.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.TabControl tabControlChat;
        private System.Windows.Forms.Panel panelCenterTop;
        private System.Windows.Forms.Button btnJoinChat;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.TextBox tbIp;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblIp;
    }
}

