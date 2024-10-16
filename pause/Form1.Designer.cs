namespace pause
{
    partial class Form1
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
            textBoxExeName = new TextBox();
            label1 = new Label();
            textBoxLog = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // textBoxExeName
            // 
            textBoxExeName.Location = new Point(89, 293);
            textBoxExeName.Name = "textBoxExeName";
            textBoxExeName.Size = new Size(401, 30);
            textBoxExeName.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 299);
            label1.Name = "label1";
            label1.Size = new Size(64, 24);
            label1.TabIndex = 2;
            label1.Text = "进程名";
            // 
            // textBoxLog
            // 
            textBoxLog.BackColor = SystemColors.GradientInactiveCaption;
            textBoxLog.Location = new Point(12, 12);
            textBoxLog.Multiline = true;
            textBoxLog.Name = "textBoxLog";
            textBoxLog.ReadOnly = true;
            textBoxLog.Size = new Size(488, 258);
            textBoxLog.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 344);
            label2.Name = "label2";
            label2.Size = new Size(214, 24);
            label2.TabIndex = 5;
            label2.Text = "F6暂停                  F7继续";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(512, 400);
            Controls.Add(label2);
            Controls.Add(textBoxLog);
            Controls.Add(label1);
            Controls.Add(textBoxExeName);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxExeName;
        private Label label1;
        private TextBox textBoxLog;
        private Label label2;
    }
}