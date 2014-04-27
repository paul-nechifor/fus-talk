using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace PN_L8_Serverul
{
    [Serializable]
    class Utilizator
    {
        public string nume;
        public string rezumatParola;
        public string ip;
        public int port;
        public bool conectat;
        public List<string> prieteni;
        // Dacă utilizatorul ăsta nu este conectat în lista asta vor fi puși cei care i-au trimis o cerere de prietenie
        // când era absent.
        public List<string> listaDePrieteniPosibili;
        public List<string> listaDeTexteLasate;
        public IPEndPoint endPoint;
    }

    class Program
    {
        public delegate void RaspunsDelegate(string mesaj);
        public static readonly RaspunsDelegate delegatulNul = (m) => {};

        private static readonly string fisierulDeSerializare = "utilizatori.ser";
        private static readonly int port = 2000;
        private static Dictionary<string, Utilizator> utilizatori;

        static void Main(string[] args)
        {
            DeserializeazaUtilizatorii();

            // Pornește bucla de ascultare într-un fir nou.
            new Thread(() => BuclaDeAscultare()).Start();

            // Dacă se închide serverul prin apăsarea de Enter în loc de X, se vor serializa utilizatorii.
            Console.ReadLine();

            lock (utilizatori)
            {
                SerializeazaUtilizatorii();
                Environment.Exit(0);
            }

        }

        private static void BuclaDeAscultare()
        {
            //TcpListener server = new TcpListener(IPAddress.Any, port);
            //server.Start();
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, port));
            server.Listen(10);

            while (true)
            {
                Socket client = server.Accept();

                new Thread(() =>
                    {
                        RaspundeLaClient(client);
                    }).Start();
            }
        }

        private static void DeserializeazaUtilizatorii()
        {
            if (File.Exists(fisierulDeSerializare))
            {
                Stream stream = File.Open(fisierulDeSerializare, FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                utilizatori = (Dictionary<string, Utilizator>)bFormatter.Deserialize(stream);
                stream.Close();

                foreach (Utilizator u in utilizatori.Values)
                    Console.WriteLine("Utilizator: {0} Parola: {1}  Prieteni: [{2}]",
                            u.nume, u.rezumatParola, String.Join(", ", u.prieteni.ToArray()));
            }
            else
            {
                utilizatori = new Dictionary<string, Utilizator>();
            }
        }

        private static void SerializeazaUtilizatorii()
        {
            // Se șterg astea pentru că trebuiesc date din nou la conectare.
            foreach (Utilizator u in utilizatori.Values)
            {
                u.ip = null;
                u.port = -1;
                u.endPoint = null;
                u.conectat = false;
            }

            Stream stream = File.Open(fisierulDeSerializare, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, utilizatori);
            stream.Close();
        }

        private static void RaspundeLaClient(Socket client)
        {
            NetworkStream stream = new NetworkStream(client, true);

            string mesaj = CitesteUnMesajDinStream(stream);
            Console.Write("De la {0}: {1}\n", client.RemoteEndPoint, mesaj);
            XmlDocument mesajXml = new XmlDocument();
            mesajXml.LoadXml(mesaj);
            XmlElement radacina = mesajXml.DocumentElement;
            string tip = radacina.GetAttribute("tip");


            if (tip == "conectare")
                RaspundeLaConectare(radacina, client, stream);
            else if (tip == "deconectare")
                RaspundeLaDeconectare(radacina, client, stream);
            else if (tip == "cererePrietenie")
                RaspundeLaCerereDePrietenie(radacina, client, stream);
            else if (tip == "lasaText")
                RaspundeLaLasaText(radacina, client, stream, mesaj);

            client.Close();
        }

        private static void RaspundeLaConectare(XmlElement radacina, Socket client, NetworkStream stream)
        {
            string nume = radacina.GetAttribute("utilizator");
            string parola = radacina.GetAttribute("parola");
            int port = int.Parse(radacina.GetAttribute("portulMeu"));

            // Dacă există utilizatorul se verifică parola.
            if (utilizatori.ContainsKey(nume))
            {
                if (utilizatori[nume].rezumatParola != RezumatSHA256(parola))
                {
                    ScrieUnMesajInStream(stream, "<mesaj tip='raspunsConectare' succes='false' motiv='parolaGresita'></mesaj>");
                }
                else
                {
                    lock (utilizatori)
                    {
                        utilizatori[nume].conectat = true;
                        utilizatori[nume].ip = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
                        utilizatori[nume].port = port;
                        utilizatori[nume].endPoint = new IPEndPoint(IPAddress.Parse(utilizatori[nume].ip), utilizatori[nume].port);
                    }

                    string raspuns = "<mesaj tip='raspunsConectare' succes='true'>" +
                            ConstruiesteListaDePrieteni(nume) + "</mesaj>";
                    ScrieUnMesajInStream(stream, raspuns);

                    AnuntaPrieteniiDeConectare(nume);

                    Console.WriteLine("S-a conectat {0}.", nume);

                    // Trebuiesc trimise cererile pe de prietenie pe care le-a primit cât a fost plecat.
                    foreach (string prieten in utilizatori[nume].listaDePrieteniPosibili)
                        TrimiteCerereaDePrietenie(prieten, nume);

                    lock (utilizatori)
                    {
                        utilizatori[nume].listaDePrieteniPosibili.Clear();
                    }

                    // Trebuiesc trimise textele pe care le-a primit cât a fost plecat.
                    foreach (string text in utilizatori[nume].listaDeTexteLasate)
                        TrimiteTextLasat(nume, text);

                    lock (utilizatori)
                    {
                        utilizatori[nume].listaDeTexteLasate.Clear();
                    }
                }
            }
            // Dacă nu există, se înregistrează.
            else
            {
                Utilizator u = new Utilizator();
                u.nume = nume;
                u.rezumatParola = RezumatSHA256(parola);
                u.ip = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
                u.port = port;
                u.conectat = true;
                u.prieteni = new List<string>();
                u.endPoint = new IPEndPoint(IPAddress.Parse(u.ip), u.port);
                u.listaDePrieteniPosibili = new List<string>();
                u.listaDeTexteLasate = new List<string>();

                lock (utilizatori)
                {
                    utilizatori[nume] = u;
                }

                string raspuns = "<mesaj tip='raspunsConectare' succes='true'>" +
                            ConstruiesteListaDePrieteni(nume) + "</mesaj>";
                ScrieUnMesajInStream(stream, raspuns);

                Console.WriteLine("S-a înregistrat {0}.", nume);
            }
        }

        private static void RaspundeLaDeconectare(XmlElement radacina, Socket client, NetworkStream stream)
        {
            string nume = radacina.GetAttribute("utilizator");

            if (utilizatori.ContainsKey(nume))
            {
                // Aici ar trebui făcută verificarea că cererea provine de la cine trebuie.
                ScrieUnMesajInStream(stream, "<mesaj tip='raspunsDeconectare' succes='true'></mesaj>");

                lock (utilizatori)
                {
                    utilizatori[nume].ip = null;
                    utilizatori[nume].port = -1;
                    utilizatori[nume].endPoint = null;
                    utilizatori[nume].conectat = false;
                }

                AnuntaPrieteniiDeDeconectare(nume);

                Console.WriteLine("S-a deconectat {0}.", nume);
            }
            else
            {
                ScrieUnMesajInStream(stream, "<mesaj tip='raspunsDeconectare' succes='false' motiv='inexistent'></mesaj>");
            }
        }

        private static void RaspundeLaCerereDePrietenie(XmlElement radacina, Socket client, NetworkStream stream)
        {
            string nume = radacina.GetAttribute("utilizator");
            string prieten = radacina.GetAttribute("prieten");

            if (!utilizatori.ContainsKey(nume))
                ScrieUnMesajInStream(stream, "<mesaj tip='raspunsCererePrietenie' succes='false' motiv='nuExisti'></mesaj>");
            else
            {
                if (!utilizatori.ContainsKey(prieten))
                    ScrieUnMesajInStream(stream, "<mesaj tip='raspunsCererePrietenie' succes='false' motiv='utilizatorInexistent'></mesaj>");
                else
                {
                    ScrieUnMesajInStream(stream, "<mesaj tip='raspunsCererePrietenie' succes='true'></mesaj>");
                    if (utilizatori[prieten].conectat)
                        TrimiteCerereaDePrietenie(nume, prieten);
                    else
                        lock (utilizatori)
                        {
                            utilizatori[prieten].listaDePrieteniPosibili.Add(nume);
                        }
                }
            }
        }

        private static void RaspundeLaLasaText(XmlElement radacina, Socket client, NetworkStream stream, string mesaj)
        {
            string deLa = radacina.GetAttribute("dela");
            string spre = radacina.GetAttribute("spre");
            string text = radacina.GetAttribute("text");
            string timp = radacina.GetAttribute("timp");

            if (!utilizatori.ContainsKey(deLa))
                ScrieUnMesajInStream(stream, "<mesaj tip='raspunsLasaMesaj' succes='false' motiv='nuExisti'></mesaj>");
            else
            {
                if (!utilizatori.ContainsKey(spre))
                    ScrieUnMesajInStream(stream, "<mesaj tip='raspunsLasaMesaj' succes='false' motiv='utilizatorInexistent'></mesaj>");
                else
                {
                    ScrieUnMesajInStream(stream, "<mesaj tip='raspunsLasaMesaj' succes='true'></mesaj>");
                    lock (utilizatori)
                    {
                        utilizatori[spre].listaDeTexteLasate.Add(mesaj);
                    }
                }
            }
        }

        private static void ScrieUnMesajInStream(Stream stream, string mesaj)
        {
            byte[] octeti = Encoding.UTF8.GetBytes(mesaj);
            byte[] marime = BitConverter.GetBytes(octeti.Length);
            stream.Write(marime, 0, 4);
            stream.Write(octeti, 0, octeti.Length);
        }

        private static string CitesteUnMesajDinStream(Stream stream)
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

        public static void TrimiteMesaj(string mesaj, IPEndPoint endPoint, RaspunsDelegate rd)
        {
            Console.WriteLine("Trimit mesajul: " + mesaj);

            new Thread(() =>
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sock.Connect(endPoint);
                    NetworkStream stream = new NetworkStream(sock, true);
                    ScrieUnMesajInStream(stream, mesaj);

                    string raspuns = CitesteUnMesajDinStream(stream);

                    Console.WriteLine("Am prmit răspunsul la mesaj: " + raspuns);

                    rd(raspuns);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Problemă la trimiterea mesajului: " + e.Message);
                }
                finally
                {
                    sock.Close();
                }
            }
            ).Start();
        }

        private static void AnuntaPrieteniiDeConectare(string utilizator)
        {
            foreach (string prieten in utilizatori[utilizator].prieteni)
                if (utilizatori[prieten].conectat)
                    AnuntaDeConectare(prieten, utilizator);
        }

        private static void AnuntaDeConectare(string peCine, string celConectat)
        {
            string msg = String.Format("<mesaj tip='aiPrietenConectat' nume='{0}' ip='{1}' port='{2}'></mesaj>", 
                    celConectat, utilizatori[celConectat].ip, utilizatori[celConectat].port);

            TrimiteMesaj(msg, utilizatori[peCine].endPoint, delegatulNul);
        }

        private static void AnuntaPrieteniiDeDeconectare(string utilizator)
        {
            foreach (string prieten in utilizatori[utilizator].prieteni)
                if (utilizatori[prieten].conectat)
                    AnuntaDeDeconectare(prieten, utilizator);
        }

        private static void AnuntaDeDeconectare(string peCine, string celDeconectat)
        {
            string msg = String.Format("<mesaj tip='aiPrietenDeconectat' nume='{0}'></mesaj>",
                    celDeconectat);

            Console.WriteLine("Deconectare {0}, {1}", peCine, celDeconectat);

            TrimiteMesaj(msg, utilizatori[peCine].endPoint, delegatulNul);
        }

        private static void TrimiteCerereaDePrietenie(string deLa, string catre)
        {
            // FOARTE IMPORTANT:
            // Dacă utilizatorul nu este conectat, trebuie memorată cererea și trimisă când se conectează.

            string msg = "<mesaj tip='aiCererePrietenie' dela='" + deLa + "'></mesaj>";
            RaspunsDelegate delegat = (mesaj) =>
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(mesaj);
                    XmlElement radacina = document.DocumentElement;

                    bool acceptat = bool.Parse(radacina.GetAttribute("acceptat"));

                    // Acum trebuie să-l anunțe pe cel care a făcut cererea de decizia celuilalt.
                    string mesajNou = String.Format("<mesaj tip='raspunsCererePrietenie' acceptat='{0}' dela='{1}'></mesaj>",
                            acceptat, catre);
                    if (utilizatori[deLa].conectat)
                        TrimiteMesaj(mesajNou, utilizatori[deLa].endPoint, Program.delegatulNul);

                    // Dacă a acceptat trebuie să-i informez de conectarea celuilalt și să-i adaug ca prieteni.
                    if (acceptat)
                    {
                        AnuntaDeConectare(deLa, catre);
                        AnuntaDeConectare(catre, deLa);

                        lock (utilizatori)
                        {
                            utilizatori[deLa].prieteni.Add(catre);
                            utilizatori[catre].prieteni.Add(deLa);
                        }
                    }
                };
            TrimiteMesaj(msg, utilizatori[catre].endPoint, delegat);
        }

        private static void TrimiteTextLasat(string spre, string mesaj)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(mesaj);
            XmlElement radacina = document.DocumentElement;

            string deLa = radacina.GetAttribute("dela");
            string text = radacina.GetAttribute("text");
            string timp = radacina.GetAttribute("timp");

            string mesajNou = String.Format("<mesaj tip='aiTextLasat' dela='{0}' text='{1}' timp='{2}'></mesaj>",
                            deLa, text, timp);

            TrimiteMesaj(mesajNou, utilizatori[spre].endPoint, delegatulNul);
        }

        private static string ConstruiesteListaDePrieteni(string utilizator)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<prieteni>");

            foreach (string prieten in utilizatori[utilizator].prieteni)
                sb.AppendFormat("<prieten nume='{0}' ip='{1}' port='{2}' conectat='{3}'/>",
                        utilizatori[prieten].nume, 
                        utilizatori[prieten].ip,
                        utilizatori[prieten].port,
                        utilizatori[prieten].conectat);

            sb.Append("</prieteni>");

            return sb.ToString();
        }

        private static string RezumatSHA256(string parola)
        {
            SHA256 h = SHA256.Create();
            byte[] octeti = h.ComputeHash(Encoding.UTF8.GetBytes(parola));
            StringBuilder rez = new StringBuilder(octeti.Length / 2);

            for (int i = 0; i < octeti.Length; i++)
                rez.Append(octeti[i].ToString("x2"));

            return rez.ToString();
        }
    }
}
