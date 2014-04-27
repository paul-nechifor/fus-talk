using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Xml;
using PN_L8_Clientul.Clase;

namespace PN_L8_Clientul
{
    public partial class FereastraPrincipala : Form
    {
        public delegate void AdaugaMesajDelegate(string mesaj);

        private readonly string fisierulDeSerializare = "server.ser";

        private Protocol protocol;

        public Dictionary<string, FereastraDeConversatie> ferestreDeConversatie;

        public FereastraPrincipala()
        {
            InitializeComponent();

            protocol = new Protocol(this);
            ferestreDeConversatie = new Dictionary<string, FereastraDeConversatie>();
        }

        private void FereastraPrincipala_Load(object sender, EventArgs e)
        {
            AflaServerul();
            AflaDateDeUtilizator();
            protocol.PornesteBuclaDeAscultare();
            protocol.Conectare();
        }

        private void FereastraPrincipala_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (protocol.conectat)
                protocol.Deconectare();
        }

        private void adaugaPrietenBtn_Click(object sender, EventArgs e)
        {
            TrimiteCererePrieten();
        }

        private void listaDePrieteni_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listaDePrieteni.IndexFromPoint(e.Location);

            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                string prieten = protocol.numeDePrieteni[index];
                if (protocol.prieteni[prieten].conectat)
                {
                    DeschideFereastraCu(prieten);
                }
                else
                {
                    MessageBox.Show("Nu poți vorbi cu un prieten care nu este conectat.");
                }
            }
        }

        private void AflaServerul()
        {
            // Verifică dacă există fișierul în care este setat serverul.
            if (File.Exists(fisierulDeSerializare))
            {
                Stream stream = File.Open(fisierulDeSerializare, FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                protocol.ipServer = (string) bFormatter.Deserialize(stream);
                protocol.portServer = (int)bFormatter.Deserialize(stream);
                stream.Close();

                AdaugaMesaj(String.Format("S-a deserializat serverul {0}:{1}.", protocol.ipServer, protocol.portServer));
            }
            // Pentru că nu există trebuie să cer.
            else
            {
                while (true)
                {
                    CereServer cs = new CereServer();
                    cs.ShowDialog(this);

                    if (!cs.RaspunsCorect)
                        continue;

                    protocol.ipServer = cs.AdresaIP;
                    protocol.portServer = cs.Portul;

                    break;
                }
                
                // Acum că l-am aflat, trebuie să-l scriu.           
                Stream stream = File.Open(fisierulDeSerializare, FileMode.Create);
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, protocol.ipServer);
                bFormatter.Serialize(stream, protocol.portServer);
                stream.Close();

                AdaugaMesaj(String.Format("S-a citit serverul {0}:{1}.", protocol.ipServer, protocol.portServer));
            }

            protocol.endPointServer = new IPEndPoint(IPAddress.Parse(protocol.ipServer), protocol.portServer);
        }

        private void AflaDateDeUtilizator()
        {
            while (true)
            {
                CereDateDeUtilizator cdu = new CereDateDeUtilizator();
                cdu.ShowDialog(this);

                if (!cdu.RaspunsCorect)
                    continue;

                protocol.utilizator = cdu.Utilizator;
                protocol.parola = cdu.Parola;
                break;
            }

            AdaugaMesaj("Numele de utilizator: " + protocol.utilizator);
        }

        private void TrimiteCererePrieten()
        {
            // Nu se face nicio verificare asupra câmpului Prieten pentru că serverul va spune dacă există sau nu acest utilizator.
            CerePrieten cp = new CerePrieten();
            cp.ShowDialog(this);

            protocol.CerereDePrietenie(cp.Prieten);
        }

        // Deschide fereastra dacă nu există.
        public void DeschideFereastraCu(string prieten)
        {
            if (this.InvokeRequired)
            {
                //Refolosesc delegatul ăsta.
                AdaugaMesajDelegate d = new AdaugaMesajDelegate(DeschideFereastraCu);
                this.Invoke(d, new object[] { prieten });
            }
            else
            {
                if (!ferestreDeConversatie.ContainsKey(prieten))
                {
                    FereastraDeConversatie fdc = new FereastraDeConversatie(protocol, prieten, this);
                    ferestreDeConversatie[prieten] = fdc;
                    fdc.Show(this);
                }
            }
        }

        // Întoarce fereastra pentru un anumit utilizator, sau o deschide dacă nu există.
        public FereastraDeConversatie FereastraPentru(string prieten)
        {
            if (!ferestreDeConversatie.ContainsKey(prieten))
                DeschideFereastraCu(prieten);

            return ferestreDeConversatie[prieten];
        }

        // Adaugă text în partea de jos a ferestrei.
        public void AdaugaMesaj(string mesaj)
        {
            if (this.mesaje.InvokeRequired)
            {
                AdaugaMesajDelegate d = new AdaugaMesajDelegate(AdaugaMesaj);
                this.Invoke(d, new object[] { mesaj });
            }
            else
                this.mesaje.AppendText(mesaj + "\n");
        }

        // Afișează lista de prieteni după ce s-a modificat sau după ce s-a creat.
        public void AfiseazaListaDePrieteni()
        {
            if (this.mesaje.InvokeRequired)
            {
                Action a = new Action(AfiseazaListaDePrieteni);
                this.Invoke(a);
            }
            else
            {
                listaDePrieteni.Items.Clear();

                foreach (string s in protocol.numeDePrieteni)
                    if (protocol.prieteni[s].conectat)
                        listaDePrieteni.Items.Add(s + " (conectat)");
                    else
                        listaDePrieteni.Items.Add(s);
            }
        }
    }
}
