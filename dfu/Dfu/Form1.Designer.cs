namespace DfuTool
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.buttonOpenPort = new System.Windows.Forms.Button();
            this.textBoxBinFilePath = new System.Windows.Forms.TextBox();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.buttonUpgrade = new System.Windows.Forms.Button();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.textBoxBaudRate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonSelectSign = new System.Windows.Forms.Button();
            this.textBoxSignPath = new System.Windows.Forms.TextBox();
            this.buttonBootVersion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(16, 409);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(84, 20);
            this.comboBoxPort.TabIndex = 4;
            this.comboBoxPort.DropDown += new System.EventHandler(this.comboBoxPort_DropDown);
            // 
            // buttonOpenPort
            // 
            this.buttonOpenPort.Location = new System.Drawing.Point(16, 442);
            this.buttonOpenPort.Name = "buttonOpenPort";
            this.buttonOpenPort.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenPort.TabIndex = 12;
            this.buttonOpenPort.Text = "打开串口";
            this.buttonOpenPort.UseVisualStyleBackColor = true;
            this.buttonOpenPort.Click += new System.EventHandler(this.buttonOpenPort_Click);
            // 
            // textBoxBinFilePath
            // 
            this.textBoxBinFilePath.Location = new System.Drawing.Point(189, 387);
            this.textBoxBinFilePath.Name = "textBoxBinFilePath";
            this.textBoxBinFilePath.ReadOnly = true;
            this.textBoxBinFilePath.Size = new System.Drawing.Size(207, 21);
            this.textBoxBinFilePath.TabIndex = 14;
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(407, 385);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectFile.TabIndex = 15;
            this.buttonSelectFile.Text = "选择文件";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(12, 10);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(470, 320);
            this.textBoxLog.TabIndex = 16;
            // 
            // buttonUpgrade
            // 
            this.buttonUpgrade.Location = new System.Drawing.Point(282, 462);
            this.buttonUpgrade.Name = "buttonUpgrade";
            this.buttonUpgrade.Size = new System.Drawing.Size(75, 23);
            this.buttonUpgrade.TabIndex = 17;
            this.buttonUpgrade.Text = "开始升级";
            this.buttonUpgrade.UseVisualStyleBackColor = true;
            this.buttonUpgrade.Click += new System.EventHandler(this.buttonStartUpgrade_Click);
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Location = new System.Drawing.Point(407, 336);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(75, 23);
            this.buttonClearLog.TabIndex = 18;
            this.buttonClearLog.Text = "清除窗口";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // textBoxBaudRate
            // 
            this.textBoxBaudRate.Location = new System.Drawing.Point(59, 374);
            this.textBoxBaudRate.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxBaudRate.Name = "textBoxBaudRate";
            this.textBoxBaudRate.Size = new System.Drawing.Size(76, 21);
            this.textBoxBaudRate.TabIndex = 52;
            this.textBoxBaudRate.Text = "9600";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 377);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 53;
            this.label5.Text = "波特率";
            // 
            // buttonSelectSign
            // 
            this.buttonSelectSign.Location = new System.Drawing.Point(407, 422);
            this.buttonSelectSign.Name = "buttonSelectSign";
            this.buttonSelectSign.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectSign.TabIndex = 55;
            this.buttonSelectSign.Text = "选择签名";
            this.buttonSelectSign.UseVisualStyleBackColor = true;
            this.buttonSelectSign.Click += new System.EventHandler(this.buttonSelectSign_Click);
            // 
            // textBoxSignPath
            // 
            this.textBoxSignPath.Location = new System.Drawing.Point(189, 424);
            this.textBoxSignPath.Name = "textBoxSignPath";
            this.textBoxSignPath.ReadOnly = true;
            this.textBoxSignPath.Size = new System.Drawing.Size(207, 21);
            this.textBoxSignPath.TabIndex = 54;
            // 
            // buttonBootVersion
            // 
            this.buttonBootVersion.Location = new System.Drawing.Point(189, 462);
            this.buttonBootVersion.Name = "buttonBootVersion";
            this.buttonBootVersion.Size = new System.Drawing.Size(75, 23);
            this.buttonBootVersion.TabIndex = 56;
            this.buttonBootVersion.Text = "BOOT版本";
            this.buttonBootVersion.UseVisualStyleBackColor = true;
            this.buttonBootVersion.Click += new System.EventHandler(this.buttonBootVersion_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 497);
            this.Controls.Add(this.buttonBootVersion);
            this.Controls.Add(this.buttonSelectSign);
            this.Controls.Add(this.textBoxSignPath);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxBaudRate);
            this.Controls.Add(this.buttonClearLog);
            this.Controls.Add(this.buttonUpgrade);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.buttonSelectFile);
            this.Controls.Add(this.textBoxBinFilePath);
            this.Controls.Add(this.buttonOpenPort);
            this.Controls.Add(this.comboBoxPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Serial DFU";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxPort;
        private System.Windows.Forms.Button buttonOpenPort;
        private System.Windows.Forms.TextBox textBoxBinFilePath;
        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button buttonUpgrade;
        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.TextBox textBoxBaudRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonSelectSign;
        private System.Windows.Forms.TextBox textBoxSignPath;
        private System.Windows.Forms.Button buttonBootVersion;
    }
}

