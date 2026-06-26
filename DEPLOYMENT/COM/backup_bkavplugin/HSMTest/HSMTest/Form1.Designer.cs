namespace HSMTest
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.listLog = new System.Windows.Forms.ListBox();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.btnSign = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnSignString = new System.Windows.Forms.Button();
            this.btnLoadCert = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDllName = new System.Windows.Forms.TextBox();
            this.btnChoice = new System.Windows.Forms.Button();
            this.txtSlotId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 106);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // listLog
            // 
            this.listLog.FormattingEnabled = true;
            this.listLog.Location = new System.Drawing.Point(93, 106);
            this.listLog.Name = "listLog";
            this.listLog.Size = new System.Drawing.Size(404, 238);
            this.listLog.TabIndex = 1;
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(12, 135);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(75, 23);
            this.btnEncrypt.TabIndex = 2;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(12, 164);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(75, 23);
            this.btnDecrypt.TabIndex = 3;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // btnSign
            // 
            this.btnSign.Location = new System.Drawing.Point(12, 193);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(75, 23);
            this.btnSign.TabIndex = 4;
            this.btnSign.Text = "Sign";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(12, 222);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(75, 23);
            this.btnVerify.TabIndex = 5;
            this.btnVerify.Text = "Verify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(12, 320);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(75, 23);
            this.btnClean.TabIndex = 6;
            this.btnClean.Text = "Clean log";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnSignString
            // 
            this.btnSignString.Location = new System.Drawing.Point(12, 251);
            this.btnSignString.Name = "btnSignString";
            this.btnSignString.Size = new System.Drawing.Size(75, 23);
            this.btnSignString.TabIndex = 7;
            this.btnSignString.Text = "Sign string";
            this.btnSignString.UseVisualStyleBackColor = true;
            this.btnSignString.Click += new System.EventHandler(this.btnSignString_Click);
            // 
            // btnLoadCert
            // 
            this.btnLoadCert.Location = new System.Drawing.Point(12, 280);
            this.btnLoadCert.Name = "btnLoadCert";
            this.btnLoadCert.Size = new System.Drawing.Size(75, 23);
            this.btnLoadCert.TabIndex = 8;
            this.btnLoadCert.Text = "Load cert";
            this.btnLoadCert.UseVisualStyleBackColor = true;
            this.btnLoadCert.Click += new System.EventHandler(this.btnLoadCert_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "HSM Dll :";
            // 
            // txtDllName
            // 
            this.txtDllName.Location = new System.Drawing.Point(93, 13);
            this.txtDllName.Name = "txtDllName";
            this.txtDllName.Size = new System.Drawing.Size(342, 20);
            this.txtDllName.TabIndex = 10;
            this.txtDllName.Text = "C:\\Program Files\\SafeNet\\Protect Toolkit C SDK\\cryptoki.dll";
            // 
            // btnChoice
            // 
            this.btnChoice.Location = new System.Drawing.Point(441, 13);
            this.btnChoice.Name = "btnChoice";
            this.btnChoice.Size = new System.Drawing.Size(56, 23);
            this.btnChoice.TabIndex = 11;
            this.btnChoice.Text = "Choice";
            this.btnChoice.UseVisualStyleBackColor = true;
            this.btnChoice.Click += new System.EventHandler(this.btnChoice_Click);
            // 
            // txtSlotId
            // 
            this.txtSlotId.Location = new System.Drawing.Point(93, 39);
            this.txtSlotId.Name = "txtSlotId";
            this.txtSlotId.Size = new System.Drawing.Size(163, 20);
            this.txtSlotId.TabIndex = 13;
            this.txtSlotId.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Slot id :";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(93, 65);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(163, 20);
            this.txtPassword.TabIndex = 15;
            this.txtPassword.Text = "12345678";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Password :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 356);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSlotId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnChoice);
            this.Controls.Add(this.txtDllName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoadCert);
            this.Controls.Add(this.btnSignString);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.btnSign);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.listLog);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hsm test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox listLog;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.Button btnSignString;
        private System.Windows.Forms.Button btnLoadCert;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDllName;
        private System.Windows.Forms.Button btnChoice;
        private System.Windows.Forms.TextBox txtSlotId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
    }
}

