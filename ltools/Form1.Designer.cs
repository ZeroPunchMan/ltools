namespace UART_demo
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
            this.textBoxReceive = new System.Windows.Forms.TextBox();
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.comboBoxBaudrate = new System.Windows.Forms.ComboBox();
            this.comboBoxDataBits = new System.Windows.Forms.ComboBox();
            this.comboBoxStopBits = new System.Windows.Forms.ComboBox();
            this.buttonOpenPort = new System.Windows.Forms.Button();
            this.buttonClearDisplay = new System.Windows.Forms.Button();
            this.checkBoxDisHex = new System.Windows.Forms.CheckBox();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.checkBoxSendHex = new System.Windows.Forms.CheckBox();
            this.checkBoxRecvNewLine = new System.Windows.Forms.CheckBox();
            this.checkBoxSendNewLine = new System.Windows.Forms.CheckBox();
            this.checkBoxDisTime = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBoxReceive
            // 
            this.textBoxReceive.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxReceive.Location = new System.Drawing.Point(10, 10);
            this.textBoxReceive.MaxLength = 655355;
            this.textBoxReceive.Multiline = true;
            this.textBoxReceive.Name = "textBoxReceive";
            this.textBoxReceive.ReadOnly = true;
            this.textBoxReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxReceive.Size = new System.Drawing.Size(732, 308);
            this.textBoxReceive.TabIndex = 0;
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(12, 356);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(84, 20);
            this.comboBoxPort.TabIndex = 4;
            this.comboBoxPort.DropDown += new System.EventHandler(this.comboBoxPort_DropDown);
            // 
            // comboBoxBaudrate
            // 
            this.comboBoxBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaudrate.FormattingEnabled = true;
            this.comboBoxBaudrate.Location = new System.Drawing.Point(108, 356);
            this.comboBoxBaudrate.Name = "comboBoxBaudrate";
            this.comboBoxBaudrate.Size = new System.Drawing.Size(83, 20);
            this.comboBoxBaudrate.TabIndex = 5;
            // 
            // comboBoxDataBits
            // 
            this.comboBoxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDataBits.FormattingEnabled = true;
            this.comboBoxDataBits.Location = new System.Drawing.Point(11, 386);
            this.comboBoxDataBits.Name = "comboBoxDataBits";
            this.comboBoxDataBits.Size = new System.Drawing.Size(85, 20);
            this.comboBoxDataBits.TabIndex = 6;
            // 
            // comboBoxStopBits
            // 
            this.comboBoxStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStopBits.FormattingEnabled = true;
            this.comboBoxStopBits.Location = new System.Drawing.Point(108, 386);
            this.comboBoxStopBits.Name = "comboBoxStopBits";
            this.comboBoxStopBits.Size = new System.Drawing.Size(83, 20);
            this.comboBoxStopBits.TabIndex = 7;
            // 
            // buttonOpenPort
            // 
            this.buttonOpenPort.Location = new System.Drawing.Point(12, 419);
            this.buttonOpenPort.Name = "buttonOpenPort";
            this.buttonOpenPort.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenPort.TabIndex = 12;
            this.buttonOpenPort.Text = "打开串口";
            this.buttonOpenPort.UseVisualStyleBackColor = true;
            this.buttonOpenPort.Click += new System.EventHandler(this.buttonOpenPort_Click);
            // 
            // buttonClearDisplay
            // 
            this.buttonClearDisplay.Location = new System.Drawing.Point(667, 326);
            this.buttonClearDisplay.Name = "buttonClearDisplay";
            this.buttonClearDisplay.Size = new System.Drawing.Size(75, 23);
            this.buttonClearDisplay.TabIndex = 13;
            this.buttonClearDisplay.Text = "清除接收";
            this.buttonClearDisplay.UseVisualStyleBackColor = true;
            this.buttonClearDisplay.Click += new System.EventHandler(this.buttonClearDisplay_Click);
            // 
            // checkBoxDisHex
            // 
            this.checkBoxDisHex.AutoSize = true;
            this.checkBoxDisHex.Location = new System.Drawing.Point(590, 326);
            this.checkBoxDisHex.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxDisHex.Name = "checkBoxDisHex";
            this.checkBoxDisHex.Size = new System.Drawing.Size(66, 16);
            this.checkBoxDisHex.TabIndex = 37;
            this.checkBoxDisHex.Text = "hex显示";
            this.checkBoxDisHex.UseVisualStyleBackColor = true;
            // 
            // textBoxSend
            // 
            this.textBoxSend.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxSend.Location = new System.Drawing.Point(217, 356);
            this.textBoxSend.MaxLength = 655355;
            this.textBoxSend.Multiline = true;
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.Size = new System.Drawing.Size(526, 94);
            this.textBoxSend.TabIndex = 38;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(667, 454);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 39;
            this.buttonSend.Text = "发送";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // checkBoxSendHex
            // 
            this.checkBoxSendHex.AutoSize = true;
            this.checkBoxSendHex.Location = new System.Drawing.Point(587, 459);
            this.checkBoxSendHex.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxSendHex.Name = "checkBoxSendHex";
            this.checkBoxSendHex.Size = new System.Drawing.Size(66, 16);
            this.checkBoxSendHex.TabIndex = 40;
            this.checkBoxSendHex.Text = "hex发送";
            this.checkBoxSendHex.UseVisualStyleBackColor = true;
            // 
            // checkBoxRecvNewLine
            // 
            this.checkBoxRecvNewLine.AutoSize = true;
            this.checkBoxRecvNewLine.Location = new System.Drawing.Point(511, 326);
            this.checkBoxRecvNewLine.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxRecvNewLine.Name = "checkBoxRecvNewLine";
            this.checkBoxRecvNewLine.Size = new System.Drawing.Size(72, 16);
            this.checkBoxRecvNewLine.TabIndex = 41;
            this.checkBoxRecvNewLine.Text = "自动换行";
            this.checkBoxRecvNewLine.UseVisualStyleBackColor = true;
            // 
            // checkBoxSendNewLine
            // 
            this.checkBoxSendNewLine.AutoSize = true;
            this.checkBoxSendNewLine.Location = new System.Drawing.Point(511, 458);
            this.checkBoxSendNewLine.Name = "checkBoxSendNewLine";
            this.checkBoxSendNewLine.Size = new System.Drawing.Size(72, 16);
            this.checkBoxSendNewLine.TabIndex = 42;
            this.checkBoxSendNewLine.Text = "回车换行";
            this.checkBoxSendNewLine.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisTime
            // 
            this.checkBoxDisTime.AutoSize = true;
            this.checkBoxDisTime.Location = new System.Drawing.Point(424, 326);
            this.checkBoxDisTime.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxDisTime.Name = "checkBoxDisTime";
            this.checkBoxDisTime.Size = new System.Drawing.Size(72, 16);
            this.checkBoxDisTime.TabIndex = 43;
            this.checkBoxDisTime.Text = "显示时间";
            this.checkBoxDisTime.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 490);
            this.Controls.Add(this.checkBoxDisTime);
            this.Controls.Add(this.checkBoxSendNewLine);
            this.Controls.Add(this.checkBoxRecvNewLine);
            this.Controls.Add(this.checkBoxSendHex);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxSend);
            this.Controls.Add(this.checkBoxDisHex);
            this.Controls.Add(this.buttonClearDisplay);
            this.Controls.Add(this.buttonOpenPort);
            this.Controls.Add(this.comboBoxStopBits);
            this.Controls.Add(this.comboBoxDataBits);
            this.Controls.Add(this.comboBoxBaudrate);
            this.Controls.Add(this.comboBoxPort);
            this.Controls.Add(this.textBoxReceive);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "串口工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxReceive;
        private System.Windows.Forms.ComboBox comboBoxPort;
        private System.Windows.Forms.ComboBox comboBoxBaudrate;
        private System.Windows.Forms.ComboBox comboBoxDataBits;
        private System.Windows.Forms.ComboBox comboBoxStopBits;
        private System.Windows.Forms.Button buttonOpenPort;
        private System.Windows.Forms.Button buttonClearDisplay;
        private System.Windows.Forms.CheckBox checkBoxDisHex;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.CheckBox checkBoxSendHex;
        private System.Windows.Forms.CheckBox checkBoxRecvNewLine;
        private System.Windows.Forms.CheckBox checkBoxSendNewLine;
        private System.Windows.Forms.CheckBox checkBoxDisTime;
    }
}

