using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcheggioThread
{
    class Ingresso
    {
        Random casuale = new Random();
        private int tempoIngresso;
        private List<Automobile> coda;
        public List<Automobile> Coda
        { get; protected set; }
        public int TempoIngresso
        { get; protected set; }

        public Ingresso()
        {
            TempoIngresso = casuale.Next(1, 5);
            Coda = new List<Automobile>();
        }
    }
}
