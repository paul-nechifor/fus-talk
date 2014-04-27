namespace PN_L8_Clientul
{
    partial class FereastraPrincipala
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
            this.adaugaPrietenBtn = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listaDePrieteni = new System.Windows.Forms.ListBox();
            this.mesaje = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // adaugaPrietenBtn
            // 
            this.adaugaPrietenBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.adaugaPrietenBtn.Location = new System.Drawing.Point(12, 572);
            this.adaugaPrietenBtn.Name = "adaugaPrietenBtn";
            this.adaugaPrietenBtn.Size = new System.Drawing.Size(128, 23);
            this.adaugaPrietenBtn.TabIndex = 2;
            this.adaugaPrietenBtn.Text = "Adaugă prieten";
            this.adaugaPrietenBtn.UseVisualStyleBackColor = true;
            this.adaugaPrietenBtn.Click += new System.EventHandler(this.adaugaPrietenBtn_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listaDePrieteni);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mesaje);
            this.splitContainer1.Size = new System.Drawing.Size(260, 554);
            this.splitContainer1.SplitterDistance = 452;
            this.splitContainer1.TabIndex = 3;
            // 
            // listaDePrieteni
            // 
            this.listaDePrieteni.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listaDePrieteni.FormattingEnabled = true;
            this.listaDePrieteni.Location = new System.Drawing.Point(0, 0);
            this.listaDePrieteni.Name = "listaDePrieteni";
            this.listaDePrieteni.Size = new System.Drawing.Size(260, 452);
            this.listaDePrieteni.TabIndex = 0;
            this.listaDePrieteni.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listaDePrieteni_MouseDoubleClick);
            // 
            // mesaje
            // 
            this.mesaje.BackColor = System.Drawing.SystemColors.Window;
            this.mesaje.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mesaje.Location = new System.Drawing.Point(0, 0);
            this.mesaje.Name = "mesaje";
            this.mesaje.ReadOnly = true;
            this.mesaje.Size = new System.Drawing.Size(260, 98);
            this.mesaje.TabIndex = 0;
            this.mesaje.Text = "";
            // 
            // FereastraPrincipala
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 607);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.adaugaPrietenBtn);
            this.Name = "FereastraPrincipala";
            this.Text = "Convorbiri Textuale 1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FereastraPrincipala_FormClosing);
            this.Load += new System.EventHandler(this.FereastraPrincipala_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button adaugaPrietenBtn;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listaDePrieteni;
        private System.Windows.Forms.RichTextBox mesaje;
    }
}

