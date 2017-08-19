namespace BCCReclaimGUI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxPW12Words = new System.Windows.Forms.TextBox();
            this.labelPWDAddr = new System.Windows.Forms.Label();
            this.labelPWPWAddr = new System.Windows.Forms.Label();
            this.labelPW12Words = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxPWPWAddr = new System.Windows.Forms.TextBox();
            this.textBoxPWDAddr = new System.Windows.Forms.TextBox();
            this.buttonPWSend = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(886, 166);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonPWSend);
            this.tabPage1.Controls.Add(this.textBoxPWDAddr);
            this.tabPage1.Controls.Add(this.textBoxPWPWAddr);
            this.tabPage1.Controls.Add(this.textBoxPW12Words);
            this.tabPage1.Controls.Add(this.labelPWDAddr);
            this.tabPage1.Controls.Add(this.labelPWPWAddr);
            this.tabPage1.Controls.Add(this.labelPW12Words);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(878, 137);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Private Wallet";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxPW12Words
            // 
            this.textBoxPW12Words.Location = new System.Drawing.Point(172, 23);
            this.textBoxPW12Words.Name = "textBoxPW12Words";
            this.textBoxPW12Words.Size = new System.Drawing.Size(695, 22);
            this.textBoxPW12Words.TabIndex = 3;
            // 
            // labelPWDAddr
            // 
            this.labelPWDAddr.AutoSize = true;
            this.labelPWDAddr.Location = new System.Drawing.Point(27, 77);
            this.labelPWDAddr.Name = "labelPWDAddr";
            this.labelPWDAddr.Size = new System.Drawing.Size(139, 17);
            this.labelPWDAddr.TabIndex = 2;
            this.labelPWDAddr.Text = "Destination Address:";
            // 
            // labelPWPWAddr
            // 
            this.labelPWPWAddr.AutoSize = true;
            this.labelPWPWAddr.Location = new System.Drawing.Point(11, 50);
            this.labelPWPWAddr.Name = "labelPWPWAddr";
            this.labelPWPWAddr.Size = new System.Drawing.Size(155, 17);
            this.labelPWPWAddr.TabIndex = 1;
            this.labelPWPWAddr.Text = "Private Wallet Address:";
            // 
            // labelPW12Words
            // 
            this.labelPW12Words.AutoSize = true;
            this.labelPW12Words.Location = new System.Drawing.Point(93, 23);
            this.labelPW12Words.Name = "labelPW12Words";
            this.labelPW12Words.Size = new System.Drawing.Size(73, 17);
            this.labelPW12Words.TabIndex = 0;
            this.labelPW12Words.Text = "12 Words:";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(973, 200);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Trading Wallet";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxPWPWAddr
            // 
            this.textBoxPWPWAddr.Location = new System.Drawing.Point(172, 47);
            this.textBoxPWPWAddr.Name = "textBoxPWPWAddr";
            this.textBoxPWPWAddr.Size = new System.Drawing.Size(695, 22);
            this.textBoxPWPWAddr.TabIndex = 4;
            // 
            // textBoxPWDAddr
            // 
            this.textBoxPWDAddr.Location = new System.Drawing.Point(172, 72);
            this.textBoxPWDAddr.Name = "textBoxPWDAddr";
            this.textBoxPWDAddr.Size = new System.Drawing.Size(695, 22);
            this.textBoxPWDAddr.TabIndex = 5;
            // 
            // buttonPWSend
            // 
            this.buttonPWSend.Location = new System.Drawing.Point(792, 100);
            this.buttonPWSend.Name = "buttonPWSend";
            this.buttonPWSend.Size = new System.Drawing.Size(75, 23);
            this.buttonPWSend.TabIndex = 6;
            this.buttonPWSend.Text = "Send";
            this.buttonPWSend.UseVisualStyleBackColor = true;
            this.buttonPWSend.Click += new System.EventHandler(this.buttonPWSend_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 190);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Reclaim BCC";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label labelPWDAddr;
        private System.Windows.Forms.Label labelPWPWAddr;
        private System.Windows.Forms.Label labelPW12Words;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxPW12Words;
        private System.Windows.Forms.Button buttonPWSend;
        private System.Windows.Forms.TextBox textBoxPWDAddr;
        private System.Windows.Forms.TextBox textBoxPWPWAddr;
    }
}

