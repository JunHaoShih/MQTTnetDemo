
namespace MQTTChatClient.View
{
    partial class ChatControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.tbChatBox = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.tlpMessageInput = new System.Windows.Forms.TableLayoutPanel();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.tbMessageInput = new System.Windows.Forms.TextBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTopic = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.tlpMessageInput.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.tbChatBox);
            this.panelMain.Controls.Add(this.panelBottom);
            this.panelMain.Controls.Add(this.panelTop);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(747, 476);
            this.panelMain.TabIndex = 0;
            // 
            // tbChatBox
            // 
            this.tbChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbChatBox.Location = new System.Drawing.Point(0, 44);
            this.tbChatBox.Multiline = true;
            this.tbChatBox.Name = "tbChatBox";
            this.tbChatBox.ReadOnly = true;
            this.tbChatBox.Size = new System.Drawing.Size(747, 384);
            this.tbChatBox.TabIndex = 2;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.tlpMessageInput);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 428);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(747, 48);
            this.panelBottom.TabIndex = 1;
            // 
            // tlpMessageInput
            // 
            this.tlpMessageInput.ColumnCount = 2;
            this.tlpMessageInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMessageInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpMessageInput.Controls.Add(this.btnSendMessage, 1, 0);
            this.tlpMessageInput.Controls.Add(this.tbMessageInput, 0, 0);
            this.tlpMessageInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMessageInput.Location = new System.Drawing.Point(0, 0);
            this.tlpMessageInput.Name = "tlpMessageInput";
            this.tlpMessageInput.RowCount = 1;
            this.tlpMessageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMessageInput.Size = new System.Drawing.Size(747, 48);
            this.tlpMessageInput.TabIndex = 0;
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSendMessage.Location = new System.Drawing.Point(650, 12);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(75, 23);
            this.btnSendMessage.TabIndex = 0;
            this.btnSendMessage.Text = "送出";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // tbMessageInput
            // 
            this.tbMessageInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMessageInput.Location = new System.Drawing.Point(10, 12);
            this.tbMessageInput.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.tbMessageInput.Name = "tbMessageInput";
            this.tbMessageInput.Size = new System.Drawing.Size(627, 23);
            this.tbMessageInput.TabIndex = 1;
            this.tbMessageInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbMessageInput_KeyPress);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblTopic);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(747, 44);
            this.panelTop.TabIndex = 0;
            // 
            // lblTopic
            // 
            this.lblTopic.AutoSize = true;
            this.lblTopic.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTopic.Location = new System.Drawing.Point(10, 13);
            this.lblTopic.Name = "lblTopic";
            this.lblTopic.Size = new System.Drawing.Size(50, 20);
            this.lblTopic.TabIndex = 0;
            this.lblTopic.Text = "Topic";
            // 
            // ChatControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "ChatControl";
            this.Size = new System.Drawing.Size(747, 476);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.tlpMessageInput.ResumeLayout(false);
            this.tlpMessageInput.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.TableLayoutPanel tlpMessageInput;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.TextBox tbMessageInput;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTopic;
        private System.Windows.Forms.TextBox tbChatBox;
    }
}
