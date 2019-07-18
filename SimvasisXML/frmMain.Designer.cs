namespace SimvasisXML
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblVat = new System.Windows.Forms.Label();
            this.txtVat = new System.Windows.Forms.TextBox();
            this.pbGenerate = new System.Windows.Forms.Button();
            this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblVat
            // 
            this.lblVat.AutoSize = true;
            this.lblVat.Location = new System.Drawing.Point(12, 9);
            this.lblVat.Name = "lblVat";
            this.lblVat.Size = new System.Drawing.Size(96, 13);
            this.lblVat.TabIndex = 0;
            this.lblVat.Text = "ΑΦΜ Επιχείρησης";
            // 
            // txtVat
            // 
            this.txtVat.Location = new System.Drawing.Point(12, 25);
            this.txtVat.MaxLength = 9;
            this.txtVat.Name = "txtVat";
            this.txtVat.Size = new System.Drawing.Size(96, 20);
            this.txtVat.TabIndex = 1;
            this.txtVat.Text = "999888777";
            // 
            // pbGenerate
            // 
            this.pbGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.pbGenerate.Location = new System.Drawing.Point(12, 123);
            this.pbGenerate.Name = "pbGenerate";
            this.pbGenerate.Size = new System.Drawing.Size(364, 23);
            this.pbGenerate.TabIndex = 3;
            this.pbGenerate.Text = "Δημιουργία αρχείου xml";
            this.pbGenerate.UseVisualStyleBackColor = true;
            this.pbGenerate.Click += new System.EventHandler(this.pbGenerate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(12, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(667, 39);
            this.label1.TabIndex = 4;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lblMessage.ForeColor = System.Drawing.Color.Navy;
            this.lblMessage.Location = new System.Drawing.Point(12, 158);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(279, 13);
            this.lblMessage.TabIndex = 5;
            this.lblMessage.Text = "ελέγξτε το ΑΦΜ, και πατήσε \"Δημιιουργία αρχείο xml\"";
            // 
            // frmMain
            // 
            this.AcceptButton = this.pbGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 184);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbGenerate);
            this.Controls.Add(this.txtVat);
            this.Controls.Add(this.lblVat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 222);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 222);
            this.Name = "frmMain";
            this.Text = "Δημιουργία Αρχείου Συμβάσεων σε xml";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblVat;
        private System.Windows.Forms.TextBox txtVat;
        private System.Windows.Forms.Button pbGenerate;
        private System.Windows.Forms.SaveFileDialog saveFileDlg;
        private System.Windows.Forms.OpenFileDialog openFileDlg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMessage;
    }
}

