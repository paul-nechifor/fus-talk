using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Media;

namespace PN_L8_Clientul.Clase
{
    public class Protocol
    {
        public delegate void RaspunsDelegate(string mesaj);

        public static readonly RaspunsDelegate delegatulNul = (m) => { };
        private readonly int ascultareDeLa = 3000;

        private FereastraPrincipala fPrincipala;

        public string ipServer;
        public int portServer;
        public IPEndPoint endPointServer;

        public string utilizator;
        public string parola;
        public int portulDeAscultare;

        public bool conectat = false;

        public Dictionary<string, Prieten> prieteni;
        public List<string> numeDePrieteni;

        public Protocol(FereastraPrincipala f)
        {
            fPrincipala = f;
        }

        public void PornesteBuclaDeAscultare()
        {
            TcpListener server = null;

            // Caută primul port de ascultare liber.
            for (portulDeAscultare = ascultareDeLa; portulDeAscultare < 65535; portulDeAscultare++)
            {
                try
                {
                    server = new TcpListener(IPAddress.Any, portulDeAscultare);
                    server.Start();
                    break;
                }
                catch (Exception e)
                {
                }
            }

            if (server == null)
            {
                MessageBox.Show("Nu s-a putut deschide un server de ascultare.", "Eroare");
                Application.Exit();
            }

            fPrincipala.AdaugaMesaj("Portul de ascultare: " + portulDeAscultare);

            Thread bucla = new Thread(() =>
                {
                    while (true)
                    {
                        TcpClient client = server.AcceptTcpClient();
                        RaspundeLaClient(client);
                    }
                });
            bucla.IsBackground = true;
            bucla.Start();
        }

        private void RaspundeLaClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            string mesaj = CitesteUnMesajDinStream(stream);
            XmlDocument mesajXml = new XmlDocument();
            mesajXml.LoadXml(mesaj);
            XmlElement radacina = mesajXml.DocumentElement;
            string tip = radacina.GetAttribute("tip");

            if (tip == "aiCererePrietenie")
                RaspundeLaAiCererePrietenie(radacina, stream);
            else if (tip == "raspunsCererePrietenie")
                RaspundeLaRaspunsCererePrietenie(radacina, stream);
            else if (tip == "aiPrietenConectat")
                RaspundeLaAiPrietenConectat(radacina, stream);
            else if (tip == "aiPrietenDeconectat")
                RaspundeLaAiPrietenDeconectat(radacina, stream);
            else if (tip == "aiTextLasat")
                RaspundeLaAiTextLasat(radacina, stream);
            else if (tip == "text")
                RaspundeLaText(radacina, stream);
            else if (tip == "fisier")
                RaspundeLaFisier(radacina, stream);
            else if (tip == "fusrodah")
                RaspundeLaFusRoDah(radacina, stream);

            client.Close();
        }

        public void RaspundeLaAiCererePrietenie(XmlElement radacina, NetworkStream stream)
        {
            string deLa = radacina.GetAttribute("dela");

            DialogResult result = MessageBox.Show("Accepți prietenia de la „" + deLa + "”?",
                    "Ai o cerere de prietenie", MessageBoxButtons.YesNo);

            string mesajRaspuns;

            if (result == DialogResult.Yes)
                mesajRaspuns = "<mesaj tip='raspunsAiCererePrietenie' acceptat='true'></mesaj>";
            else
                mesajRaspuns = "<mesaj tip='raspunsAiCererePrietenie' acceptat='false'></mesaj>";

            ScrieUnMesajInStream(stream, mesajRaspuns);
        }

        public void RaspundeLaRaspunsCererePrietenie(XmlElement radacina, NetworkStream stream)
        {
            bool acceptat = bool.Parse(radacina.GetAttribute("acceptat"));
            string deLa = radacina.GetAttribute("dela");

            if (acceptat)
                MessageBox.Show("Ți-a fost acceptată cererea de prietenie de către „" + deLa + "”.");
            else
                MessageBox.Show("Ți-a fost respinsă cererea de prietenie de către „" + deLa + "”.");
        }

        public void RaspundeLaAiPrietenConectat(XmlElement radacina, NetworkStream stream)
        {
            ScrieUnMesajInStream(stream, "<mesaj tip='confirmareAiPrietenConectat'></mesaj>");

            Prieten p = new Prieten();
            p.nume = radacina.GetAttribute("nume");
            p.ip = radacina.GetAttribute("ip");
            p.port = int.Parse(radacina.GetAttribute("port"));
            p.endPoint = new IPEndPoint(IPAddress.Parse(p.ip), p.port);
            p.conectat = true;

            prieteni[p.nume] = p;

            RefaListaDePrieteni();

            fPrincipala.AdaugaMesaj("S-a conectat " + p.nume + ".");

            if (fPrincipala.ferestreDeConversatie.ContainsKey(p.nume))
                fPrincipala.ferestreDeConversatie[p.nume].AdaugaTextAutomat(p.nume + " a revenit.");
        }

        public void RaspundeLaAiPrietenDeconectat(XmlElement radacina, NetworkStream stream)
        {
            ScrieUnMesajInStream(stream, "<mesaj tip='confirmareAiPrietenDeconectat'></mesaj>");

            string nume = radacina.GetAttribute("nume");
            prieteni[nume].conectat = false;
            prieteni[nume].ip = null;
            prieteni[nume].port = -1;
            prieteni[nume].endPoint = null;

            RefaListaDePrieteni();

            fPrincipala.AdaugaMesaj("S-a deconectat " + nume + ".");

            if (fPrincipala.ferestreDeConversatie.ContainsKey(nume))
                fPrincipala.ferestreDeConversatie[nume].AdaugaTextAutomat(nume + " a plecat.");
        }

        public void RaspundeLaAiTextLasat(XmlElement radacina, NetworkStream stream)
        {
            ScrieUnMesajInStream(stream, "<mesaj tip='confirmareAiTextLasat'></mesaj>");

            string deLa = radacina.GetAttribute("dela");
            string text = radacina.GetAttribute("text");
            string timp = radacina.GetAttribute("timp");

            fPrincipala.FereastraPentru(deLa).AdaugaText(text, deLa + " (" + timp + "): ");
        }

        public void RaspundeLaText(XmlElement radacina, NetworkStream stream)
        {
            ScrieUnMesajInStream(stream, "<mesaj tip='confirmareText'></mesaj>");

            string deLa = radacina.GetAttribute("utilizator");
            string text = radacina.GetAttribute("text");

            fPrincipala.FereastraPentru(deLa).AdaugaText(text, deLa + " (" + DateTime.Now.ToString("HH:mm:ss") + "): ");
        }

        public void RaspundeLaFisier(XmlElement radacina, NetworkStream stream)
        {
            string deLa = radacina.GetAttribute("utilizator");
            string denumire = radacina.GetAttribute("denumire");
            long marime = long.Parse(radacina.GetAttribute("marime"));

            DialogResult result = MessageBox.Show(String.Format("Accepți fișierul „{0}” cu mărimea {1} de la {2}?",
                    denumire, marime, deLa),
                    "Acceptare fișier", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                fPrincipala.Invoke((MethodInvoker)delegate()
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.FileName = denumire;
                    if (dialog.ShowDialog(fPrincipala) == DialogResult.OK)
                    {
                        fPrincipala.FereastraPentru(deLa).AdaugaTextAutomat("Ai acceptat fișierul „" + denumire + "” care va fi salvat în " +
                                new FileInfo(dialog.FileName).FullName + ".");
                        int portul = AcceptaFisier(dialog.FileName, deLa);
                        string mesajRaspuns = "<mesaj tip='raspunsFisier' acceptat='true' port='" + portul + "'></mesaj>";
                        ScrieUnMesajInStream(stream, mesajRaspuns);
                    }
                    else
                    {
                        // Sau poate alt mesaj aici.
                        string mesajRaspuns = "<mesaj tip='raspunsFisier' acceptat='false'></mesaj>";
                        ScrieUnMesajInStream(stream, mesajRaspuns);
                    }
                });
            }
            else
            {
                string mesajRaspuns = "<mesaj tip='raspunsFisier' acceptat='false'></mesaj>";
                ScrieUnMesajInStream(stream, mesajRaspuns);
            }
        }

        public void RaspundeLaFusRoDah(XmlElement radacina, NetworkStream stream)
        {
            string prieten = radacina.GetAttribute("utilizator");
            fPrincipala.FereastraPentru(prieten).FusRoDah();
        }

        public void Conectare()
        {
            string msg = String.Format("<mesaj tip='conectare' utilizator='{0}' parola='{1}' portulMeu='{2}'></mesaj>",
                    utilizator, parola, portulDeAscultare);

            RaspunsDelegate delegat = (mesaj) =>
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(mesaj);
                XmlElement radacina = document.DocumentElement;

                bool succes = bool.Parse(radacina.GetAttribute("succes"));

                if (!succes)
                {
                    string motiv = radacina.GetAttribute("motiv");
                    if (motiv == "parolaGresita")
                        MessageBox.Show("Parola este greșită.");
                    else
                        MessageBox.Show("Eroare: " + motiv);

                    Application.Exit();
                    return;
                }
                conectat = true;
                IncarcaPrietenii((XmlElement)radacina.FirstChild);
            };

            TrimiteMesaj(msg, endPointServer, delegat);
        }

        public void Deconectare()
        {
            if (!conectat)
                return;

            conectat = false;

            string msg = String.Format("<mesaj tip='deconectare' utilizator='{0}'></mesaj>", utilizator);
            TrimiteMesaj(msg, endPointServer, delegatulNul);
        }

        public void CerereDePrietenie(string prieten)
        {
            string msg = String.Format("<mesaj tip='cererePrietenie' utilizator='{0}' prieten='{1}'></mesaj>",
                    utilizator, prieten);

            RaspunsDelegate delegat = (mesaj) =>
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(mesaj);
                XmlElement radacina = document.DocumentElement;

                bool succes = bool.Parse(radacina.GetAttribute("succes"));

                if (!succes)
                {
                    string motiv = radacina.GetAttribute("motiv");
                    if (motiv == "utilizatorInexistent")
                        MessageBox.Show("Nu există utilizatorul „" + prieten + "”.");
                    else
                        MessageBox.Show("Eroare: " + motiv);
                }
                else
                {
                    MessageBox.Show("Cererea a fost trimisă.");
                }
            };

            TrimiteMesaj(msg, endPointServer, delegat);
        }

        public void TrimiteTextulLaPrieten(string prieten, string textul)
        {
            if (prieteni[prieten].conectat)
            {
                string msg = String.Format("<mesaj tip='text' utilizator='{0}' text='{1}'></mesaj>", utilizator, textul);
                TrimiteMesaj(msg, prieteni[prieten].endPoint, delegatulNul);
            }
            else
            {
                string msg = String.Format("<mesaj tip='lasaText' dela='{0}' spre='{1}' text='{2}' timp='{3}'></mesaj>",
                        utilizator, prieten, textul, DateTime.Now.ToString("HH:mm:ss"));
                TrimiteMesaj(msg, endPointServer, delegatulNul);

                MessageBox.Show("Mesajul va fi primit de „" + prieten + "” la reconectare.");
            }
        }

        public void TrimiteFisierLaPrieten(string prieten, string denumire, long marime, string locatie)
        {
            if (!prieteni[prieten].conectat)
            {
                MessageBox.Show("Nu poți trimite fișierul pentru că „" + prieten, "” nu este conectat.");
                return;
            }

            string msg = String.Format("<mesaj tip='fisier' utilizator='{0}' denumire='{1}' marime='{2}'></mesaj>",
                    utilizator, denumire, marime);

            RaspunsDelegate delegat = (mesaj) =>
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(mesaj);
                    XmlElement radacina = document.DocumentElement;

                    bool acceptat = bool.Parse(radacina.GetAttribute("acceptat"));

                    if (acceptat)
                    {
                        int port = int.Parse(radacina.GetAttribute("port"));
                        Stream stream = File.Open(locatie, FileMode.Open);
                        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        sock.Connect(new IPEndPoint(IPAddress.Parse(prieteni[prieten].ip), port));
                        NetworkStream nStream = new NetworkStream(sock, true);
                        stream.CopyTo(nStream);
                        sock.Close();
                        stream.Close();
                        fPrincipala.FereastraPentru(prieten).AdaugaTextAutomat("S-a terminat încărcarea fișierului „" + denumire + "”.");
                    }
                    else
                    {
                        fPrincipala.FereastraPentru(prieten).AdaugaTextAutomat("A refuzat să primească fișierul „"+denumire+"”.");
                    }
                };

            TrimiteMesaj(msg, prieteni[prieten].endPoint, delegat);
        }

        public void TrimiteFusRoDah(string prieten)
        {
            if (!prieteni[prieten].conectat)
            {
                MessageBox.Show("Nu poți trimite Fus ro dah pentru că „" + prieten, "” nu este conectat.");
                return;
            }
            string msg = String.Format("<mesaj tip='fusrodah' utilizator='{0}'></mesaj>", utilizator);
            TrimiteMesaj(msg, prieteni[prieten].endPoint, delegatulNul);
        }

        private int AcceptaFisier(string locatieSalvare, string deLa)
        {
            TcpListener server = null;
            int portPrimire;
            for (portPrimire = 4000; portPrimire < 65535; portPrimire++)
            {
                try
                {
                    server = new TcpListener(IPAddress.Any, portPrimire);
                    server.Start();
                    break;
                }
                catch (Exception e) { }
            }

            if (server == null)
                return -1;

            Thread fir = new Thread(() =>
            {
                TcpClient client = server.AcceptTcpClient();
                Stream stream = File.Open(locatieSalvare, FileMode.Create);
                client.GetStream().CopyTo(stream);
                client.Close();
                stream.Close();
                fPrincipala.FereastraPentru(deLa).AdaugaTextAutomat("S-a terminat descărcarea fișierului în „" 
                        + new FileInfo(locatieSalvare).FullName + "”.");
            });
            fir.IsBackground = true;
            fir.Start();

            return portPrimire;
        }

        public void TrimiteMesaj(string mesaj, IPEndPoint endPoint, RaspunsDelegate rd)
        {
            new Thread(() =>
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sock.Connect(endPoint);
                    NetworkStream stream = new NetworkStream(sock, true);
                    ScrieUnMesajInStream(stream, mesaj);

                    string raspuns = CitesteUnMesajDinStream(stream);

                    rd(raspuns);
                }
                catch (SocketException e)
                {
                    fPrincipala.AdaugaMesaj("Problemă la trimiterea mesajului: " + e.Message);
                }
                finally
                {
                    sock.Close();
                }
            }
            ).Start();
        }

        private void IncarcaPrietenii(XmlElement element)
        {
            prieteni = new Dictionary<string, Prieten>();

            foreach (XmlElement prieten in element.ChildNodes)
            {
                Prieten p = new Prieten();
                p.nume = prieten.GetAttribute("nume");
                p.conectat = bool.Parse(prieten.GetAttribute("conectat"));
                p.ip = prieten.GetAttribute("ip");
                p.port = int.Parse(prieten.GetAttribute("port"));
                if (p.conectat)
                    p.endPoint = new IPEndPoint(IPAddress.Parse(p.ip), p.port);
                else
                    p.endPoint = null;

                prieteni[p.nume] = p;
            }

            RefaListaDePrieteni();
        }

        private void RefaListaDePrieteni()
        {
            numeDePrieteni = new List<string>();

            foreach (Prieten p in prieteni.Values)
                numeDePrieteni.Add(p.nume);

            numeDePrieteni.Sort();

            fPrincipala.AfiseazaListaDePrieteni();
        }

        private void ScrieUnMesajInStream(Stream stream, string mesaj)
        {
            byte[] octeti = Encoding.UTF8.GetBytes(mesaj);
            byte[] marime = BitConverter.GetBytes(octeti.Length);
            stream.Write(marime, 0, 4);
            stream.Write(octeti, 0, octeti.Length);
        }

        private string CitesteUnMesajDinStream(Stream stream)
        {
            byte[] marime = new byte[4];
            stream.Read(marime, 0, 4);
            int numarDeOcteti = BitConverter.ToInt32(marime, 0);

            byte[] octeti = new byte[numarDeOcteti];

            int citit = 0;

            while (citit < numarDeOcteti)
            {
                int n = stream.Read(octeti, citit, numarDeOcteti - citit);
                citit += n;
            }

            return Encoding.UTF8.GetString(octeti);
        }
    }
}
