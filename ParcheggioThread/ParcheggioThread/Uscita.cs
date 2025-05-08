using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcheggioThread
{
    class Uscita
    {
        Random casuale = new Random();
        private int tempoUscita;
        private List<Automobile> coda;
        public List<Automobile> Coda
        { get; protected set; }
        public int TempoUscita
        { get; protected set; }

        public Uscita()
        {
            TempoUscita = casuale.Next(1,10);
            Coda = new List<Automobile>();
        }
    }
}
