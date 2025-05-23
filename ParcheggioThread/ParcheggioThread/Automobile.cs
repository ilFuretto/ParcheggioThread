﻿using System;
using System.Collections.Generic;

namespace ParcheggioThread
{
    class Automobile : MioThread
    {
        private static Random random = new Random();
        private string modello;
        private string marca;
        private string targa;
        private Parcheggio parcheggio;
        private int tempoSosta;
        private ListBox ingrBox;
        private ListBox uscitaBox;

        public string Modello { get { return modello; } }
        public string Marca { get { return marca; } }
        public string Targa { get { return targa; } }
        public Parcheggio Parcheggio { get { return parcheggio; } }
        public int TempoSosta { get { return tempoSosta; } }




        public Automobile(Parcheggio parcheggio)
        {
            this.parcheggio = parcheggio;
            this.tempoSosta = random.Next(5, 10);
            this.marca = GeneraMarca();
            this.modello = GeneraModello(this.marca);
            this.targa = GeneraTarga();

            Avvia(this.CicloAutomobile);
        }


        public override string ToString()
        {
            return $"{marca} {modello} {targa} ({tempoSosta}s)";
        }

        public void CicloAutomobile()
        {
            Ingresso ingresso = Entra();
            int nSem = parcheggio.IngressoList.IndexOf(ingresso);
            while (parcheggio.Form.CentroBox.Items.Count >= parcheggio.Posti)
                Thread.Sleep(2000);
            try
            {
                
                parcheggio.SemIngressi[nSem].WaitOne();
            }
            finally
            {
                Thread.Sleep(ingresso.TempoIngresso * 1000);
                parcheggio.SemIngressi[nSem].Release();
                ingresso.Coda.Remove(this);
                parcheggio.AggiornaListBox(ingrBox, ToString(), TempoSosta, false);
                parcheggio.SemCentrale.WaitOne();
                parcheggio.AggiornaListBox(parcheggio.Form.CentroBox, ToString(), TempoSosta, true);
                parcheggio.SemCentrale.WaitOne();
                parcheggio.AggiornaListBox(parcheggio.Form.CentroBox, ToString(), TempoSosta, false);
            }
            int uscitaIndex = random.Next(0, parcheggio.UscitaList.Count);
            Uscita uscita = parcheggio.UscitaList[uscitaIndex];
            uscitaBox = parcheggio.GetUscitaBox(uscitaIndex);
            Esci(uscita);
            try 
            {
                parcheggio.SemUscite[uscitaIndex].WaitOne();
            }
            finally 
            {
                Thread.Sleep(uscita.TempoUscita * 1000);
                parcheggio.SemUscite[uscitaIndex].Release();
                uscita.Coda.Remove(this);
                parcheggio.AggiornaListBox(uscitaBox, ToString(), TempoSosta, false);
            }
        }




        private Ingresso Entra()
        {
            int ingressoIndex = accesso("i");
            Ingresso ingresso = parcheggio.IngressoList[ingressoIndex];
            ingresso.Coda.Add(this);
            ingrBox = parcheggio.GetIngressoBox(ingressoIndex);
            parcheggio.AggiornaListBox(ingrBox, ToString(), TempoSosta, true);
            return ingresso;
        }

        private void Esci(Uscita uscita)
        {
            uscita.Coda.Add(this);
            parcheggio.AggiornaListBox(uscitaBox, ToString(), TempoSosta, true);
        }

        private int accesso(string sbarra)
        {
            Random casuale = new Random();
            if (sbarra == "i")
                return casuale.Next(0, parcheggio.IngressoList.Count);
            else
                return -1;
        }

        private static string GeneraMarca()
        {
            List<string> marche = new List<string> { "Fiat", "Ford", "BMW", "Audi", "Toyota" };
            return marche[random.Next(marche.Count)];
        }

        private static string GeneraModello(string marca)
        {
            Dictionary<string, List<string>> modelli = new Dictionary<string, List<string>>()
            {
                { "Fiat", new List<string> { "Punto", "500", "Panda" } },
                { "Ford", new List<string> { "Focus", "Fiesta", "Kuga" } },
                { "BMW", new List<string> { "320i", "X3", "X5" } },
                { "Audi", new List<string> { "A3", "A4", "Q5" } },
                { "Toyota", new List<string> { "Yaris", "Corolla", "Rav4" } }
            };

            if (modelli.ContainsKey(marca))
            {
                var lista = modelli[marca];
                return lista[random.Next(lista.Count)];
            }

            return "Modello";
        }

        private static string GeneraTarga()
        {
            string lettere = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return $"{lettere[random.Next(26)]}{lettere[random.Next(26)]}{random.Next(100, 999)}{lettere[random.Next(26)]}{lettere[random.Next(26)]}";
        }
    }
}
