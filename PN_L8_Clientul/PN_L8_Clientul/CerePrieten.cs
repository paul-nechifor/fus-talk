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
    public partial class CerePrieten : Form
    {
        public string Prieten;

        public CerePrieten()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Prieten = prietenTb.Text;
            this.Close();
        }
    }
}
