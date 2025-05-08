using System.Threading;

namespace ParcheggioThread
{
    class MioThread
    {
        private Thread automobile;

        public void Avvia(Action azione)
        {
            automobile = new Thread(new ThreadStart(azione));
            automobile.Start();
        }

        public void Join()
        {
            automobile.Join();
        }
    }

}
