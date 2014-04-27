namespace PN_L8_Clientul
{
    partial class FereastraDeConversatie
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
            this.components = new System.ComponentModel.Container();
            this.conversatieRtb = new System.Windows.Forms.RichTextBox();
            this.intrareRtb = new System.Windows.Forms.RichTextBox();
            this.trimiteBtn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.meniuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trimiteFișierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fusRoDahToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // conversatieRtb
            // 
            this.conversatieRtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.conversatieRtb.BackColor = System.Drawing.SystemColors.Window;
            this.conversatieRtb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.conversatieRtb.Location = new System.Drawing.Point(12, 27);
            this.conversatieRtb.Name = "conversatieRtb";
            this.conversatieRtb.ReadOnly = true;
            this.conversatieRtb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.conversatieRtb.Size = new System.Drawing.Size(434, 236);
            this.conversatieRtb.TabIndex = 2;
            this.conversatieRtb.Text = "";
            // 
            // intrareRtb
            // 
            this.intrareRtb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.intrareRtb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.intrareRtb.Location = new System.Drawing.Point(12, 269);
            this.intrareRtb.Name = "intrareRtb";
            this.intrareRtb.Size = new System.Drawing.Size(359, 42);
            this.intrareRtb.TabIndex = 0;
            this.intrareRtb.Text = "";
            this.intrareRtb.KeyUp += new System.Windows.Forms.KeyEventHandler(this.intrareRtb_KeyUp);
            // 
            // trimiteBtn
            // 
            this.trimiteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.trimiteBtn.Location = new System.Drawing.Point(377, 269);
            this.trimiteBtn.Name = "trimiteBtn";
            this.trimiteBtn.Size = new System.Drawing.Size(69, 42);
            this.trimiteBtn.TabIndex = 1;
            this.trimiteBtn.Text = "Trimite";
            this.trimiteBtn.UseVisualStyleBackColor = true;
            this.trimiteBtn.Click += new System.EventHandler(this.trimiteBtn_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meniuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(458, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // meniuToolStripMenuItem
            // 
            this.meniuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trimiteFișierToolStripMenuItem,
            this.fusRoDahToolStripMenuItem});
            this.meniuToolStripMenuItem.Name = "meniuToolStripMenuItem";
            this.meniuToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.meniuToolStripMenuItem.Text = "Meniu";
            // 
            // trimiteFișierToolStripMenuItem
            // 
            this.trimiteFișierToolStripMenuItem.Name = "trimiteFișierToolStripMenuItem";
            this.trimiteFișierToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.trimiteFișierToolStripMenuItem.Text = "Trimite fișier...";
            this.trimiteFișierToolStripMenuItem.Click += new System.EventHandler(this.trimiteFișierToolStripMenuItem_Click);
            // 
            // fusRoDahToolStripMenuItem
            // 
            this.fusRoDahToolStripMenuItem.Name = "fusRoDahToolStripMenuItem";
            this.fusRoDahToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.fusRoDahToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.fusRoDahToolStripMenuItem.Text = "Fus ro dah!";
            this.fusRoDahToolStripMenuItem.Click += new System.EventHandler(this.fusRoDahToolStripMenuItem_Click);
            // 
            // FereastraDeConversatie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 323);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.trimiteBtn);
            this.Controls.Add(this.intrareRtb);
            this.Controls.Add(this.conversatieRtb);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FereastraDeConversatie";
            this.Text = "FereastraDeConversatie";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FereastraDeConversatie_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox conversatieRtb;
        private System.Windows.Forms.RichTextBox intrareRtb;
        private System.Windows.Forms.Button trimiteBtn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem meniuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trimiteFișierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fusRoDahToolStripMenuItem;
    }
}