namespace Server
{
    partial class FrmServer
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
            this.btnStart = new System.Windows.Forms.Button();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lbPort = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.lblReceive = new System.Windows.Forms.Label();
            this.gblog = new System.Windows.Forms.GroupBox();
            this.lvLog = new System.Windows.Forms.ListView();
            this.gbsend = new System.Windows.Forms.GroupBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbMsg = new System.Windows.Forms.TextBox();
            this.btnFrm = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.pbReceive = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pbSend = new System.Windows.Forms.ProgressBar();
            this.lblSend = new System.Windows.Forms.Label();
            this.gb.SuspendLayout();
            this.gblog.SuspendLayout();
            this.gbsend.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb
            // 
            this.gb.Controls.Add(this.btnStart);
            this.gb.Controls.Add(this.tbPort);
            this.gb.Controls.Add(this.lbPort);
            this.gb.Controls.Add(this.tbIP);
            this.gb.Location = new System.Drawing.Point(0, 12);
            this.gb.Name = "gb";
            this.gb.Size = new System.Drawing.Size(222, 148);
            this.gb.TabIndex = 0;
            this.gb.TabStop = false;
            this.gb.Text = "设置";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(76, 108);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(76, 61);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(61, 25);
            this.tbPort.TabIndex = 2;
            this.tbPort.Text = "6666";
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(15, 64);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(39, 15);
            this.lbPort.TabIndex = 0;
            this.lbPort.Text = "Port";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(76, 24);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(118, 25);
            this.tbIP.TabIndex = 1;
            this.tbIP.Text = "10.241.51.144";
            // 
            // lblReceive
            // 
            this.lblReceive.AutoSize = true;
            this.lblReceive.Location = new System.Drawing.Point(1019, 634);
            this.lblReceive.Name = "lblReceive";
            this.lblReceive.Size = new System.Drawing.Size(0, 15);
            this.lblReceive.TabIndex = 0;
            // 
            // gblog
            // 
            this.gblog.Controls.Add(this.lvLog);
            this.gblog.Location = new System.Drawing.Point(222, 12);
            this.gblog.Name = "gblog";
            this.gblog.Size = new System.Drawing.Size(877, 619);
            this.gblog.TabIndex = 2;
            this.gblog.TabStop = false;
            this.gblog.Text = "日志记录";
            // 
            // lvLog
            // 
            this.lvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLog.Location = new System.Drawing.Point(3, 21);
            this.lvLog.Name = "lvLog";
            this.lvLog.Size = new System.Drawing.Size(871, 595);
            this.lvLog.TabIndex = 0;
            this.lvLog.UseCompatibleStateImageBehavior = false;
            this.lvLog.View = System.Windows.Forms.View.Details;
            // 
            // gbsend
            // 
            this.gbsend.Controls.Add(this.btnSend);
            this.gbsend.Controls.Add(this.tbMsg);
            this.gbsend.Location = new System.Drawing.Point(0, 187);
            this.gbsend.Name = "gbsend";
            this.gbsend.Size = new System.Drawing.Size(222, 236);
            this.gbsend.TabIndex = 2;
            this.gbsend.TabStop = false;
            this.gbsend.Text = "发送消息";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(-2, 180);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(224, 56);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbMsg
            // 
            this.tbMsg.Location = new System.Drawing.Point(0, 18);
            this.tbMsg.Multiline = true;
            this.tbMsg.Name = "tbMsg";
            this.tbMsg.Size = new System.Drawing.Size(222, 163);
            this.tbMsg.TabIndex = 4;
            // 
            // btnFrm
            // 
            this.btnFrm.Location = new System.Drawing.Point(119, 596);
            this.btnFrm.Name = "btnFrm";
            this.btnFrm.Size = new System.Drawing.Size(75, 23);
            this.btnFrm.TabIndex = 3;
            this.btnFrm.Text = "抖窗";
            this.btnFrm.UseVisualStyleBackColor = true;
            this.btnFrm.Click += new System.EventHandler(this.btnFrm_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(119, 554);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 4;
            this.btnSendFile.Text = "发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // pbReceive
            // 
            this.pbReceive.Location = new System.Drawing.Point(94, 631);
            this.pbReceive.Name = "pbReceive";
            this.pbReceive.Size = new System.Drawing.Size(919, 23);
            this.pbReceive.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 634);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "接收文件";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 661);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "发送文件";
            // 
            // pbSend
            // 
            this.pbSend.Location = new System.Drawing.Point(94, 661);
            this.pbSend.Name = "pbSend";
            this.pbSend.Size = new System.Drawing.Size(919, 23);
            this.pbSend.TabIndex = 1;
            // 
            // lblSend
            // 
            this.lblSend.AutoSize = true;
            this.lblSend.Location = new System.Drawing.Point(1019, 665);
            this.lblSend.Name = "lblSend";
            this.lblSend.Size = new System.Drawing.Size(0, 15);
            this.lblSend.TabIndex = 5;
            // 
            // FrmServer
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 685);
            this.Controls.Add(this.lblSend);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbSend);
            this.Controls.Add(this.pbReceive);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnFrm);
            this.Controls.Add(this.gbsend);
            this.Controls.Add(this.lblReceive);
            this.Controls.Add(this.gblog);
            this.Controls.Add(this.gb);
            this.Name = "FrmServer";
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmServer_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gb.ResumeLayout(false);
            this.gb.PerformLayout();
            this.gblog.ResumeLayout(false);
            this.gbsend.ResumeLayout(false);
            this.gbsend.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb;
        private System.Windows.Forms.Label lblReceive;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox gblog;
        private System.Windows.Forms.ListView lvLog;
        private System.Windows.Forms.GroupBox gbsend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbMsg;
        private System.Windows.Forms.Button btnFrm;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.ProgressBar pbReceive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pbSend;
        private System.Windows.Forms.Label lblSend;
    }
}

