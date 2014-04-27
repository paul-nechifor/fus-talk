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
    public partial class CereDateDeUtilizator : Form
    {
        public bool RaspunsCorect = false;
        public string Utilizator;
        public string Parola;


        public CereDateDeUtilizator()
        {
            InitializeComponent();
        }

        private void gataBtn_Click(object sender, EventArgs e)
        {
            string mesajDeEroare = "";

            if (this.utilizatorTb.Text.Length <= 0)
                mesajDeEroare = "Trebuie un nume de utilizator.";
            if (this.parolaTb.Text.Length <= 0)
                mesajDeEroare = "Trebuie o parola.";

            if (mesajDeEroare.Length > 0)
                MessageBox.Show(mesajDeEroare, "Eroare");
            else
            {
                Utilizator = this.utilizatorTb.Text;
                Parola = this.parolaTb.Text;
                RaspunsCorect = true;
                this.Close();
            }
        }
    }
}
