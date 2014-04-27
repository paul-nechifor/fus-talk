using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PN_L8_Clientul.Clase;
using System.IO;
using System.Media;

namespace PN_L8_Clientul
{
    public partial class FereastraDeConversatie : Form
    {
        private delegate void AdaugaTextDelegate(string text, string deLa);
        private delegate void AdaugaTextAutomatDelegate(string text);

        private Protocol protocol;
        private string prieten;
        private FereastraPrincipala fPrincipala;

        private Font fontNormal;
        private Font fontMesajAutomat;
        private Font fontBold;

        private int miscaLaX;
        private int miscaLaY;
        private Timer miscaTimer;

        public FereastraDeConversatie(Protocol protocol, string prieten, FereastraPrincipala f)
        {
            InitializeComponent();

            fontNormal = conversatieRtb.SelectionFont;
            fontMesajAutomat = new Font(fontNormal, FontStyle.Italic);
            fontBold = new Font(fontNormal, FontStyle.Bold);

            this.protocol = protocol;
            this.prieten = prieten;
            this.fPrincipala = f;

            this.Text = protocol.utilizator + " cu " + prieten;
            AdaugaTextAutomat("S-a deschis conversația cu " + prieten);
        }

        private void trimiteBtn_Click(object sender, EventArgs e)
        {
            TrimiteMesaj();
        }

        private void intrareRtb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                TrimiteMesaj();
        }

        private void FereastraDeConversatie_FormClosing(object sender, FormClosingEventArgs e)
        {
            fPrincipala.ferestreDeConversatie.Remove(prieten);
        }

        // Adaugă text de la el sau de la prieten.
        public void AdaugaText(string text, string deLa)
        {
            if (conversatieRtb.InvokeRequired)
            {
                AdaugaTextDelegate d = new AdaugaTextDelegate(AdaugaText);
                this.Invoke(d, new object[] { text, deLa });
            }
            else
            {
                conversatieRtb.SelectionFont = fontBold;
                conversatieRtb.AppendText(deLa);
                conversatieRtb.SelectionFont = fontNormal;
                conversatieRtb.AppendText(text + "\n");
                conversatieRtb.SelectionStart = conversatieRtb.Text.Length;
                conversatieRtb.ScrollToCaret();
            }
        }

        // Adaugă text care nu provine de la un utilizator uman.
        public void AdaugaTextAutomat(string text)
        {
            if (conversatieRtb.InvokeRequired)
            {
                AdaugaTextAutomatDelegate d = new AdaugaTextAutomatDelegate(AdaugaTextAutomat);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                conversatieRtb.SelectionFont = fontMesajAutomat;
                conversatieRtb.AppendText(text);
                conversatieRtb.SelectionFont = fontNormal;
                conversatieRtb.AppendText("\n");
                conversatieRtb.SelectionStart = conversatieRtb.Text.Length;
                conversatieRtb.ScrollToCaret();
            }
        }

        private void TrimiteMesaj()
        {
            string textul = intrareRtb.Text.Trim();
            intrareRtb.ResetText();

            AdaugaText(textul, protocol.utilizator + " (" + DateTime.Now.ToString("HH:mm:ss") + "): ");
            protocol.TrimiteTextulLaPrieten(prieten, textul);
        }

        // Face acțiunea Fus ro dah!
        public void FusRoDah()
        {
            if (conversatieRtb.InvokeRequired)
            {
                Action d = new Action(FusRoDah);
                this.Invoke(d);
            }
            else
            {
                AdaugaTextAutomat("Fus ro dah!");
                SoundPlayer simpleSound = new SoundPlayer("fusrodah.wav");
                simpleSound.Play();

                // Calculeaza mărimile.
                Rectangle r = Screen.GetBounds(this);
                int wScreen = r.Width;
                int hScreen = r.Height;
                int mwFereastra = this.Width / 2;
                int mhFereastra = this.Height / 2;

                int[] alegere;

                if (mwFereastra + this.Left < wScreen / 2) // Fereastra e în partea stângă.
                {
                    if (mhFereastra + this.Top < hScreen / 2) // Stânga sus.
                        alegere = new int[] { 2, 3, 4 };
                    else // Stânga jos.
                        alegere = new int[] { 1, 2, 4 };
                }
                else // Fereastra e în partea dreaptă.
                {
                    if (mhFereastra + this.Top < hScreen / 2) // Dreapta sus.
                        alegere = new int[] { 1, 3, 4 };
                    else // Dreapta jos.
                        alegere = new int[] { 1, 2, 3 };
                }

                int a = alegere[new Random().Next(0, 3)];

                System.Threading.Thread.Sleep(500);

                if (a == 1) // Stânga sus.
                    TrimiteFereastraLa(0, 0);
                else if (a == 2) // Dreapta sus.
                    TrimiteFereastraLa(wScreen - this.Width, 0);
                else if (a == 3) // Stânga jos.
                    TrimiteFereastraLa(0, hScreen - this.Height);
                else
                    TrimiteFereastraLa(wScreen - this.Width, hScreen - this.Height);
            }
        }

        // Mișcă fereastra până când a ajuns unde trebuie.
        private void Misca(object sender, EventArgs e)
        {
            if (Math.Abs(this.Top - miscaLaY) == 1 && Math.Abs(this.Left - miscaLaX) == 1)
            {
                miscaTimer.Stop();
                this.Top = miscaLaY;
                this.Left = miscaLaX;
            }
            else
            {
                int plusX = Math.Abs(miscaLaX - this.Left) / 2;
                int plusY = Math.Abs(miscaLaY - this.Top) / 2;
                if (this.Top < miscaLaY)
                    this.Top += plusY;
                else
                    this.Top -= plusY;
                if (this.Left < miscaLaX)
                    this.Left += plusX;
                else
                    this.Left -= plusX;
            }
        }

        // Animează trimiterea ferestrei la un anumit punct.
        private void TrimiteFereastraLa(int x, int y)
        {
            if (miscaTimer != null)
                miscaTimer.Stop();
            miscaLaX = x;
            miscaLaY = y;
            miscaTimer = new Timer();
            miscaTimer.Tick += new EventHandler(Misca);
            miscaTimer.Interval = 20;
            miscaTimer.Enabled = true;
            miscaTimer.Start(); 
        }

        private void trimiteFișierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo info = new FileInfo(dialog.FileName);

                protocol.TrimiteFisierLaPrieten(prieten, info.Name, info.Length, dialog.FileName);
            }
        }

        private void fusRoDahToolStripMenuItem_Click(object sender, EventArgs e)
        {
            protocol.TrimiteFusRoDah(prieten);
        }
    }
}
