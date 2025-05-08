using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ParcheggioThread
{
    class Parcheggio
    {
        private Form1 form;
        private int posti;
        private Semaphore semIngresso = new Semaphore(1, 1);
        private Semaphore semUscita = new Semaphore(1, 1);
        private Random rnd = new Random();

        
        private List<Ingresso> ingressoList;
        private List<Uscita> uscitaList;
        private List<ListBox> ingressiBoxes = new List<ListBox>();
        private List<ListBox> usciteBoxes = new List<ListBox>();

        public List<Ingresso> IngressoList { get { return ingressoList; } }
        public List<Uscita> UscitaList { get { return uscitaList; } }
        public Form1 Form { get { return form; } }
        public Semaphore SemIngresso {  get { return semIngresso; } }
        public Semaphore SemUscita {  get { return semUscita; } }
        public int Posti {  get { return posti; } }

        public Parcheggio(Form1 form, int posti)
        {
            this.form = form;
            this.posti = posti;
            ingressoList = new List<Ingresso>();
            uscitaList = new List<Uscita>();

            int numIngressi = rnd.Next(1, 6);
            int numUscite = rnd.Next(1, 6);

            for (int i = 0; i < numIngressi; i++)
            {
                Ingresso ingresso = new Ingresso();
                ingressoList.Add(ingresso);

                ListBox box = new ListBox();
                box.Width = 165;
                box.Height = 180;
                box.Name = "lstIngresso" + i;
                ingressiBoxes.Add(box);
                form.FlowPanelIngressi.Controls.Add(box);
            }

            for (int i = 0; i < numUscite; i++)
            {
                Uscita uscita = new Uscita();
                uscitaList.Add(uscita);

                ListBox box = new ListBox();
                box.Width = 165;
                box.Height = 180;
                box.Name = "lstUscita" + i;
                usciteBoxes.Add(box);
                form.FlowPanelUscite.Controls.Add(box);
            }
            AvviaAuto();
        }

        private void AvviaAuto()
        {
            Automobile auto = new Automobile( this);
        }

        public ListBox GetIngressoBox(int index)
        {
            return ingressiBoxes[index];
        }
        public ListBox GetUscitaBox(int index)
        {
            return usciteBoxes[index];
        }
        public void AggiornaListBox(ListBox box, string testo, int tempoSosta, bool aggiungi)
        {
            if (box == form.CentroBox && aggiungi)
            {
                for (; tempoSosta > 0; tempoSosta--)
                {
                    box.Items.Add(testo);
                    Thread.Sleep(1000);
                    box.Invoke(new MethodInvoker(delegate 
                    {
                        
                        box.Items.Remove(testo);
                        char[] chars = testo.ToCharArray();
                        chars[chars.Length - 3] = (char)(chars[chars.Length - 3] - 1);

                        testo = new string(chars);
                    }));
                }
                return;
            }
            box.Invoke(new MethodInvoker(delegate
                {
                    if (aggiungi)
                        {
                            box.Items.Add(testo);
                        }
                    else
                        box.Items.Remove(testo);
                }));
        }
    }
}
