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
        private List <Semaphore> semIngressi = new List<Semaphore>();
        private List <Semaphore> semUscite = new List<Semaphore>();
        private Random rnd = new Random();
        private List<Ingresso> ingressoList;
        private List<Uscita> uscitaList;
        private List<ListBox> ingressiBoxes = new List<ListBox>();
        private List<ListBox> usciteBoxes = new List<ListBox>();

        public List<Ingresso> IngressoList { get { return ingressoList; } }
        public List<Uscita> UscitaList { get { return uscitaList; } }
        public Form1 Form { get { return form; } }
        public List <Semaphore> SemIngressi {  get { return semIngressi; } }
        public List<Semaphore> SemUscite {  get { return semUscite; } }
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
                SemIngressi.Add(new Semaphore(1, 1));
                ListBox box = new ListBox();
                box.Width = 200;
                box.Height = 180;
                box.Name = "lstIngresso" + i;
                ingressiBoxes.Add(box);
                form.FlowPanelIngressi.Controls.Add(box);
            }

            for (int i = 0; i < numUscite; i++)
            {
                Uscita uscita = new Uscita();
                uscitaList.Add(uscita);
                SemUscite.Add(new Semaphore(1, 1));
                ListBox box = new ListBox();
                box.Width = 200;
                box.Height = 180;
                box.Name = "lstUscita" + i;
                usciteBoxes.Add(box);
                form.FlowPanelUscite.Controls.Add(box);
            }
            form.Shown += Form_Shown;
        }
        private void Form_Shown(object sender, EventArgs e)
        {
            AvviaAuto();
        }

        private void AvviaAuto()
        {
            new Thread(() =>
            {
                for (int i = 10; i > 0; i--)
                {
                    Automobile auto = new Automobile(this);
                    Thread.Sleep(1000);
                }
            }).Start();
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
