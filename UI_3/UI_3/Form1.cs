using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_3
{
    public partial class Form1 : Form
    {
        public Point nova = new Point(); // nova tocka pri risanju znaka
        public Point stara = new Point(); // prejsna tocka
        public Graphics g;
        public Pen pen = new Pen(Color.Black, 5);
        public List<Point> vektor = new List<Point>(); // tu imamo shranjene vse tocke
        bool poteka_ucenje = true; // ce se se program uci ali ne
        public int stevilo_vektorjev = 16; // koliko vektorjev bomo uporabili
        public int stevilo_nevronov = 12; // koliko nevronov
        public double stopnja_ucenja = 0.25; // stopnja ucenja
        public double dovoljena_napaka = 0.005; // napaka
        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            panel1.Invalidate(); // pocistimo 
            stara = e.Location; // si shranimo lokacijo kjer smo zaceli oz stisnili
            vektor.Add(stara); // dodamo v vektor
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // ce drzimo levi gumb na miski
            {
                nova = e.Location; // za vsak premik dobimo novo lokavijo
                vektor.Add(nova); // dodamo v vektor
                g.DrawLine(pen, stara, nova); // narisemo crto med novo in staro
                stara = nova; // si prepisemo staro
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Invalidate(); // pocistimo panel
            /*
            MessageBox.Show("Zaj bom ponovno narisel!");
            for (int i = 0; i < vektor.Count()-1; i++) {
                g.DrawLine(pen, vektor[i], vektor[i + 1]);
            }
            */
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            // ko spustimo misko
            if (poteka_ucenje) // ce poteka ucenje
            {
                // moramo vpisati kateri znak smo narisali
                MessageBox.Show("Tukaj bos vpisal keri znak je"); 
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            poteka_ucenje = true; // damo da poteka ucenje
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            poteka_ucenje = false; // damo da ne poteka ucenje
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("St vektorjev: " + stevilo_vektorjev);
            stevilo_vektorjev = (int)numericUpDown1.Value; // pridobimo vrednost
            //MessageBox.Show("St vektorjev: " + stevilo_vektorjev);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("St nev: " + stevilo_nevronov);
            stevilo_nevronov = (int)numericUpDown2.Value; // pridobimo vrednost
            //MessageBox.Show("St nev: " + stevilo_nevronov);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("St ucenja: " + stopnja_ucenja);
            stopnja_ucenja = (double)numericUpDown3.Value; // pridobimo vrednost
            //MessageBox.Show("St ucenja: " + stopnja_ucenja);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Napaka: " + dovoljena_napaka);
            dovoljena_napaka = (double)numericUpDown4.Value; // pridobimo vrednost
            //MessageBox.Show("Napaka: " + dovoljena_napaka);
        }
    }
}
