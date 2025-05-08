using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ParcheggioThread
{
    public partial class Form1 : Form
    {
        public FlowLayoutPanel FlowPanelIngressi { get; private set; }
        public FlowLayoutPanel FlowPanelUscite { get; private set; }
        public ListBox CentroBox { get; private set; }
        public Random random;

        public Form1()
        {
            InitializeComponent();
            InizializzaUI();

            random = new Random();

            // Il Parcheggio gestisce tutta la simulazione
            Parcheggio parcheggio = new Parcheggio(this, random.Next(10,15));
            CheckForIllegalCrossThreadCalls = false;
        }

        private void InizializzaUI()
        {
            this.Width = 1200;
            this.Height = 600;
            this.Text = "Simulazione Parcheggio";

            FlowPanelIngressi = new FlowLayoutPanel();
            FlowPanelIngressi.Dock = DockStyle.Left;
            FlowPanelIngressi.Width = 300;
            FlowPanelIngressi.AutoScroll = true;
            this.Controls.Add(FlowPanelIngressi);

            CentroBox = new ListBox();
            //CentroBox.Dock = DockStyle.Fill;
            CentroBox.Location = new Point(350, 0);
            CentroBox.Size = new Size(420, 680);
            this.Controls.Add(CentroBox);

            FlowPanelUscite = new FlowLayoutPanel();
            FlowPanelUscite.Location = new Point(820, 0);
            FlowPanelUscite.Size = new Size(250, this.Height);
            FlowPanelUscite.AutoScroll = true; 
            this.Controls.Add(FlowPanelUscite);
        }
    }
}
