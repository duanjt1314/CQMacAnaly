namespace CQMacAnaly
{
    partial class Form1
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
            this.btnSelectTmpPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTmpPath = new System.Windows.Forms.TextBox();
            this.btnSelectDataPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDataPath = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnStartMAC = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_mac = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtSelect = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.nudTimes = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudSecond = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAnalyMac = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSecond)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectTmpPath
            // 
            this.btnSelectTmpPath.Location = new System.Drawing.Point(524, 39);
            this.btnSelectTmpPath.Name = "btnSelectTmpPath";
            this.btnSelectTmpPath.Size = new System.Drawing.Size(45, 23);
            this.btnSelectTmpPath.TabIndex = 12;
            this.btnSelectTmpPath.Text = ".....";
            this.btnSelectTmpPath.UseVisualStyleBackColor = true;
            this.btnSelectTmpPath.Click += new System.EventHandler(this.btnSelectTmpPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "解压临时目录：";
            // 
            // txtTmpPath
            // 
            this.txtTmpPath.Location = new System.Drawing.Point(169, 39);
            this.txtTmpPath.Name = "txtTmpPath";
            this.txtTmpPath.Size = new System.Drawing.Size(348, 21);
            this.txtTmpPath.TabIndex = 10;
            // 
            // btnSelectDataPath
            // 
            this.btnSelectDataPath.Location = new System.Drawing.Point(524, 12);
            this.btnSelectDataPath.Name = "btnSelectDataPath";
            this.btnSelectDataPath.Size = new System.Drawing.Size(45, 23);
            this.btnSelectDataPath.TabIndex = 9;
            this.btnSelectDataPath.Text = ".....";
            this.btnSelectDataPath.UseVisualStyleBackColor = true;
            this.btnSelectDataPath.Click += new System.EventHandler(this.btnSelectDataPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "要解压的数据文件：";
            // 
            // txtDataPath
            // 
            this.txtDataPath.Location = new System.Drawing.Point(169, 12);
            this.txtDataPath.Name = "txtDataPath";
            this.txtDataPath.Size = new System.Drawing.Size(348, 21);
            this.txtDataPath.TabIndex = 7;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 106);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(697, 368);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnStartMAC);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txt_mac);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(689, 342);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "查找MAC被采集的信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnStartMAC
            // 
            this.btnStartMAC.Location = new System.Drawing.Point(228, 199);
            this.btnStartMAC.Name = "btnStartMAC";
            this.btnStartMAC.Size = new System.Drawing.Size(75, 23);
            this.btnStartMAC.TabIndex = 17;
            this.btnStartMAC.Text = "开始";
            this.btnStartMAC.UseVisualStyleBackColor = true;
            this.btnStartMAC.Click += new System.EventHandler(this.btnStartMAC_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(440, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(239, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "横杠格式忽略大小写，多个MAC英文逗号分隔";
            // 
            // txt_mac
            // 
            this.txt_mac.Location = new System.Drawing.Point(133, 6);
            this.txt_mac.Multiline = true;
            this.txt_mac.Name = "txt_mac";
            this.txt_mac.Size = new System.Drawing.Size(301, 163);
            this.txt_mac.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "输入要分析的MAC：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.txtSelect);
            this.tabPage2.Controls.Add(this.txtFileName);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.nudTimes);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.nudSecond);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.btnAnalyMac);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(689, 342);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "MAC伴随分析";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtSelect
            // 
            this.txtSelect.Location = new System.Drawing.Point(466, 243);
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.Size = new System.Drawing.Size(56, 23);
            this.txtSelect.TabIndex = 26;
            this.txtSelect.Text = "选择";
            this.txtSelect.UseVisualStyleBackColor = true;
            this.txtSelect.Click += new System.EventHandler(this.txtSelect_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(153, 243);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(291, 21);
            this.txtFileName.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(51, 248);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "要解析的文件";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(414, 282);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "解析日志文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // nudTimes
            // 
            this.nudTimes.Location = new System.Drawing.Point(353, 19);
            this.nudTimes.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudTimes.Name = "nudTimes";
            this.nudTimes.Size = new System.Drawing.Size(91, 21);
            this.nudTimes.TabIndex = 22;
            this.nudTimes.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(292, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 21;
            this.label8.Text = "同行次数";
            // 
            // nudSecond
            // 
            this.nudSecond.Location = new System.Drawing.Point(153, 17);
            this.nudSecond.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudSecond.Name = "nudSecond";
            this.nudSecond.Size = new System.Drawing.Size(121, 21);
            this.nudSecond.TabIndex = 20;
            this.nudSecond.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "同行间隔时间(秒)";
            // 
            // btnAnalyMac
            // 
            this.btnAnalyMac.Location = new System.Drawing.Point(369, 71);
            this.btnAnalyMac.Name = "btnAnalyMac";
            this.btnAnalyMac.Size = new System.Drawing.Size(75, 23);
            this.btnAnalyMac.TabIndex = 18;
            this.btnAnalyMac.Text = "开始";
            this.btnAnalyMac.UseVisualStyleBackColor = true;
            this.btnAnalyMac.Click += new System.EventHandler(this.btnAnalyMac_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblInfo.Location = new System.Drawing.Point(85, 73);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(71, 12);
            this.lblInfo.TabIndex = 14;
            this.lblInfo.Text = "等待执行...";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(414, 131);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 23);
            this.button2.TabIndex = 27;
            this.button2.Text = "查找目标MAC";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(36, 131);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(372, 81);
            this.textBox1.TabIndex = 28;
            this.textBox1.Text = "输入mac,多个mac用逗号分割";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 486);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnSelectTmpPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTmpPath);
            this.Controls.Add(this.btnSelectDataPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDataPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "重庆特征采集分析工具";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSecond)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectTmpPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTmpPath;
        private System.Windows.Forms.Button btnSelectDataPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDataPath;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txt_mac;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStartMAC;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnAnalyMac;
        private System.Windows.Forms.NumericUpDown nudSecond;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudTimes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button txtSelect;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
    }
}

