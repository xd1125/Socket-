namespace Client
{
    partial class FrmClient
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
            this.gb = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lbPort = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.lbIP = new System.Windows.Forms.Label();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tbMsg = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.gbOperation = new System.Windows.Forms.GroupBox();
            this.btnFrm = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.lbMsg = new System.Windows.Forms.Label();
            this.pbSend = new System.Windows.Forms.ProgressBar();
            this.pbReceive = new System.Windows.Forms.ProgressBar();
            this.lblSend = new System.Windows.Forms.Label();
            this.lblReceive = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gb.SuspendLayout();
            this.gbOperation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb
            // 
            this.gb.Controls.Add(this.btnConnect);
            this.gb.Controls.Add(this.tbPort);
            this.gb.Controls.Add(this.lbPort);
            this.gb.Controls.Add(this.tbIP);
            this.gb.Controls.Add(this.lbIP);
            this.gb.Location = new System.Drawing.Point(12, 12);
            this.gb.Name = "gb";
            this.gb.Size = new System.Drawing.Size(507, 77);
            this.gb.TabIndex = 0;
            this.gb.TabStop = false;
            this.gb.Text = "设置";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(415, 32);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(312, 29);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(73, 25);
            this.tbPort.TabIndex = 2;
            this.tbPort.Text = "6666";
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(260, 34);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(39, 15);
            this.lbPort.TabIndex = 0;
            this.lbPort.Text = "Port";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(62, 29);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(180, 25);
            this.tbIP.TabIndex = 1;
            this.tbIP.Text = "10.241.51.144";
            // 
            // lbIP
            // 
            this.lbIP.AutoSize = true;
            this.lbIP.Location = new System.Drawing.Point(22, 35);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(23, 15);
            this.lbIP.TabIndex = 0;
            this.lbIP.Text = "IP";
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(0, 24);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.Size = new System.Drawing.Size(507, 255);
            this.tbLog.TabIndex = 1;
            // 
            // tbMsg
            // 
            this.tbMsg.Location = new System.Drawing.Point(80, 328);
            this.tbMsg.Name = "tbMsg";
            this.tbMsg.Size = new System.Drawing.Size(329, 25);
            this.tbMsg.TabIndex = 4;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(415, 328);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.button1_Click);
            // 
            // gbOperation
            // 
            this.gbOperation.Controls.Add(this.pbReceive);
            this.gbOperation.Controls.Add(this.pbSend);
            this.gbOperation.Controls.Add(this.btnFrm);
            this.gbOperation.Controls.Add(this.btnSendFile);
            this.gbOperation.Controls.Add(this.label1);
            this.gbOperation.Controls.Add(this.lblReceive);
            this.gbOperation.Controls.Add(this.lblSend);
            this.gbOperation.Controls.Add(this.tbLog);
            this.gbOperation.Controls.Add(this.btnSend);
            this.gbOperation.Controls.Add(this.tbMsg);
            this.gbOperation.Controls.Add(this.lbMsg);
            this.gbOperation.Location = new System.Drawing.Point(12, 95);
            this.gbOperation.Name = "gbOperation";
            this.gbOperation.Size = new System.Drawing.Size(507, 453);
            this.gbOperation.TabIndex = 3;
            this.gbOperation.TabStop = false;
            this.gbOperation.Text = "操作";
            // 
            // btnFrm
            // 
            this.btnFrm.Location = new System.Drawing.Point(415, 411);
            this.btnFrm.Name = "btnFrm";
            this.btnFrm.Size = new System.Drawing.Size(75, 23);
            this.btnFrm.TabIndex = 7;
            this.btnFrm.Text = "抖窗";
            this.btnFrm.UseVisualStyleBackColor = true;
            this.btnFrm.Click += new System.EventHandler(this.btnFrm_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(415, 368);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 6;
            this.btnSendFile.Text = "发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // lbMsg
            // 
            this.lbMsg.AutoSize = true;
            this.lbMsg.Location = new System.Drawing.Point(6, 332);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(61, 15);
            this.lbMsg.TabIndex = 0;
            this.lbMsg.Text = "信   息";
            // 
            // pbSend
            // 
            this.pbSend.Location = new System.Drawing.Point(0, 368);
            this.pbSend.Name = "pbSend";
            this.pbSend.Size = new System.Drawing.Size(369, 23);
            this.pbSend.TabIndex = 8;
            // 
            // pbReceive
            // 
            this.pbReceive.Location = new System.Drawing.Point(80, 411);
            this.pbReceive.Name = "pbReceive";
            this.pbReceive.Size = new System.Drawing.Size(289, 23);
            this.pbReceive.TabIndex = 8;
            // 
            // lblSend
            // 
            this.lblSend.AutoSize = true;
            this.lblSend.Location = new System.Drawing.Point(375, 372);
            this.lblSend.Name = "lblSend";
            this.lblSend.Size = new System.Drawing.Size(0, 15);
            this.lblSend.TabIndex = 0;
            // 
            // lblReceive
            // 
            this.lblReceive.AutoSize = true;
            this.lblReceive.Location = new System.Drawing.Point(375, 415);
            this.lblReceive.Name = "lblReceive";
            this.lblReceive.Size = new System.Drawing.Size(0, 15);
            this.lblReceive.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 415);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "接收文件：";
            // 
            // FrmClient
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 582);
            this.Controls.Add(this.gbOperation);
            this.Controls.Add(this.gb);
            this.Name = "FrmClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmClient_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gb.ResumeLayout(false);
            this.gb.PerformLayout();
            this.gbOperation.ResumeLayout(false);
            this.gbOperation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.TextBox tbMsg;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.GroupBox gbOperation;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnFrm;
        private System.Windows.Forms.ProgressBar pbReceive;
        private System.Windows.Forms.ProgressBar pbSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblReceive;
        private System.Windows.Forms.Label lblSend;
    }
}

