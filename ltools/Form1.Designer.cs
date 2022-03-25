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
            this.checkBoxNewLine = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBoxReceive
            // 
            this.textBoxReceive.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxReceive.Location = new System.Drawing.Point(13, 13);
            this.textBoxReceive.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxReceive.MaxLength = 655355;
            this.textBoxReceive.Multiline = true;
            this.textBoxReceive.Name = "textBoxReceive";
            this.textBoxReceive.ReadOnly = true;
            this.textBoxReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxReceive.Size = new System.Drawing.Size(975, 409);
            this.textBoxReceive.TabIndex = 0;
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(16, 474);
            this.comboBoxPort.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(111, 24);
            this.comboBoxPort.TabIndex = 4;
            this.comboBoxPort.DropDown += new System.EventHandler(this.comboBoxPort_DropDown);
            // 
            // comboBoxBaudrate
            // 
            this.comboBoxBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaudrate.FormattingEnabled = true;
            this.comboBoxBaudrate.Location = new System.Drawing.Point(144, 474);
            this.comboBoxBaudrate.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxBaudrate.Name = "comboBoxBaudrate";
            this.comboBoxBaudrate.Size = new System.Drawing.Size(109, 24);
            this.comboBoxBaudrate.TabIndex = 5;
            // 
            // comboBoxDataBits
            // 
            this.comboBoxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDataBits.FormattingEnabled = true;
            this.comboBoxDataBits.Location = new System.Drawing.Point(15, 515);
            this.comboBoxDataBits.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxDataBits.Name = "comboBoxDataBits";
            this.comboBoxDataBits.Size = new System.Drawing.Size(112, 24);
            this.comboBoxDataBits.TabIndex = 6;
            // 
            // comboBoxStopBits
            // 
            this.comboBoxStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStopBits.FormattingEnabled = true;
            this.comboBoxStopBits.Location = new System.Drawing.Point(144, 515);
            this.comboBoxStopBits.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxStopBits.Name = "comboBoxStopBits";
            this.comboBoxStopBits.Size = new System.Drawing.Size(109, 24);
            this.comboBoxStopBits.TabIndex = 7;
            // 
            // buttonOpenPort
            // 
            this.buttonOpenPort.Location = new System.Drawing.Point(16, 559);
            this.buttonOpenPort.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenPort.Name = "buttonOpenPort";
            this.buttonOpenPort.Size = new System.Drawing.Size(100, 31);
            this.buttonOpenPort.TabIndex = 12;
            this.buttonOpenPort.Text = "打开串口";
            this.buttonOpenPort.UseVisualStyleBackColor = true;
            this.buttonOpenPort.Click += new System.EventHandler(this.buttonOpenPort_Click);
            // 
            // buttonClearDisplay
            // 
            this.buttonClearDisplay.Location = new System.Drawing.Point(889, 435);
            this.buttonClearDisplay.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClearDisplay.Name = "buttonClearDisplay";
            this.buttonClearDisplay.Size = new System.Drawing.Size(100, 31);
            this.buttonClearDisplay.TabIndex = 13;
            this.buttonClearDisplay.Text = "清除接收";
            this.buttonClearDisplay.UseVisualStyleBackColor = true;
            this.buttonClearDisplay.Click += new System.EventHandler(this.buttonClearDisplay_Click);
            // 
            // checkBoxDisHex
            // 
            this.checkBoxDisHex.AutoSize = true;
            this.checkBoxDisHex.Location = new System.Drawing.Point(787, 435);
            this.checkBoxDisHex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxDisHex.Name = "checkBoxDisHex";
            this.checkBoxDisHex.Size = new System.Drawing.Size(80, 21);
            this.checkBoxDisHex.TabIndex = 37;
            this.checkBoxDisHex.Text = "hex显示";
            this.checkBoxDisHex.UseVisualStyleBackColor = true;
            // 
            // textBoxSend
            // 
            this.textBoxSend.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxSend.Location = new System.Drawing.Point(289, 474);
            this.textBoxSend.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSend.MaxLength = 655355;
            this.textBoxSend.Multiline = true;
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.Size = new System.Drawing.Size(700, 124);
            this.textBoxSend.TabIndex = 38;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(889, 606);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(100, 31);
            this.buttonSend.TabIndex = 39;
            this.buttonSend.Text = "发送";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // checkBoxSendHex
            // 
            this.checkBoxSendHex.AutoSize = true;
            this.checkBoxSendHex.Location = new System.Drawing.Point(783, 612);
            this.checkBoxSendHex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxSendHex.Name = "checkBoxSendHex";
            this.checkBoxSendHex.Size = new System.Drawing.Size(80, 21);
            this.checkBoxSendHex.TabIndex = 40;
            this.checkBoxSendHex.Text = "hex发送";
            this.checkBoxSendHex.UseVisualStyleBackColor = true;
            // 
            // checkBoxNewLine
            // 
            this.checkBoxNewLine.AutoSize = true;
            this.checkBoxNewLine.Location = new System.Drawing.Point(681, 435);
            this.checkBoxNewLine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxNewLine.Name = "checkBoxNewLine";
            this.checkBoxNewLine.Size = new System.Drawing.Size(86, 21);
            this.checkBoxNewLine.TabIndex = 41;
            this.checkBoxNewLine.Text = "自动换行";
            this.checkBoxNewLine.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 653);
            this.Controls.Add(this.checkBoxNewLine);
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
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "串口IAP";
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
        private System.Windows.Forms.CheckBox checkBoxNewLine;
    }
}

