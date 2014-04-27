using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PN_L8_Clientul
{
    public partial class CereServer : Form
    {
        public int Portul;
        public string AdresaIP;
        public bool RaspunsCorect = false;

        public CereServer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mesajDeEroare = "";

            if (!int.TryParse(this.portTb.Text, out Portul))
                mesajDeEroare = "Portul nu este număr.";
            else if (Portul < 0 || Portul > 65535)
                mesajDeEroare = "Portul nu este în intervalul valid.";

            this.ipTb.Text = this.ipTb.Text.Trim();
            string[] numereIp = this.ipTb.Text.Split('.');

            if (numereIp.Length != 4)
                mesajDeEroare = "O adresă IP are patru numere.";
            else
            {
                int nimic;
                for (int i = 0; i < 4; i++)
                {
                    if (!int.TryParse(numereIp[i], out nimic))
                        mesajDeEroare = "Trebuiesc patru numere separate de puncte.";
                    if (nimic < 0 || nimic > 255)
                        mesajDeEroare = "Numerele sunt între 0 și 255.";
                }
            }

            AdresaIP = this.ipTb.Text;

            if (mesajDeEroare.Length > 0)
                MessageBox.Show(mesajDeEroare, "Eroare");
            else
            {
                RaspunsCorect = true;
                this.Close();
            }
        }
    }
}
