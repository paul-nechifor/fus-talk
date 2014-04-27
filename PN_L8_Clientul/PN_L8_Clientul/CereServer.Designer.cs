namespace PN_L8_Clientul
{
    partial class CereServer
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
            this.label1 = new System.Windows.Forms.Label();
            this.ipTb = new System.Windows.Forms.TextBox();
            this.portTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gataBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Adresa IP a serverului:";
            // 
            // ipTb
            // 
            this.ipTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ipTb.Location = new System.Drawing.Point(126, 12);
            this.ipTb.Name = "ipTb";
            this.ipTb.Size = new System.Drawing.Size(218, 20);
            this.ipTb.TabIndex = 1;
            // 
            // portTb
            // 
            this.portTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.portTb.Location = new System.Drawing.Point(126, 38);
            this.portTb.Name = "portTb";
            this.portTb.Size = new System.Drawing.Size(218, 20);
            this.portTb.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Portul:";
            // 
            // gataBtn
            // 
            this.gataBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gataBtn.Location = new System.Drawing.Point(126, 79);
            this.gataBtn.Name = "gataBtn";
            this.gataBtn.Size = new System.Drawing.Size(107, 23);
            this.gataBtn.TabIndex = 3;
            this.gataBtn.Text = "Gata";
            this.gataBtn.UseVisualStyleBackColor = true;
            this.gataBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // CereServer
            // 
            this.AcceptButton = this.gataBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 111);
            this.Controls.Add(this.gataBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.portTb);
            this.Controls.Add(this.ipTb);
            this.Controls.Add(this.label1);
            this.Name = "CereServer";
            this.Text = "Care este serverul central?";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ipTb;
        private System.Windows.Forms.TextBox portTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button gataBtn;
    }
}