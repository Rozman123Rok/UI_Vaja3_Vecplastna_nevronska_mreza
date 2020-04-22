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
        public List<Tocka> vektor = new List<Tocka>(); // tu imamo shranjene vse tocke
        public List<Tocka> vektor_2 = new List<Tocka>(); // tu imamo shranjene vse tocke
        bool poteka_ucenje = true; // ce se se program uci ali ne
        public int stevilo_vektorjev = 16; // koliko vektorjev bomo uporabili
        public int stevilo_nevronov = 12; // koliko nevronov
        public double stopnja_ucenja = 0.25; // stopnja ucenja
        public double dovoljena_napaka = 0.005; // napaka
        int maxX = 587; // velikost panel  na kateri risem
        int maxY = 426; // velikost panel  na kateri risem

        public List<Tocka> Tocke = new List<Tocka>(); // tu imamo shranjene vse tocke

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
            double x = (double)stara.X;
            double y = (double)stara.Y;
            vektor.Add(new Tocka(x, y)); // dodamo v vektor
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // ce drzimo levi gumb na miski
            {
                nova = e.Location; // za vsak premik dobimo novo lokavijo
                double x = (double)nova.X;
                double y = (double)nova.Y;
                vektor.Add(new Tocka(x,y)); // dodamo v vektor
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
                vektorizacija();
                normalizacija();
            }
            vektor.Clear();
            vektor_2.Clear();
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

        private void vektorizacija()
        {
            List<Tocka> temp1 = new List<Tocka>(); // tu imamo shranjene vse tocke
            List<Tocka> temp2 = new List<Tocka>(); // tu imamo shranjene vse tocke
            vektor_2.Add(vektor[0]); // prvi element ostane fix
            Tocka prva = new Tocka(vektor[0].X, vektor[0].Y); // prva tocka ki ostane fix
            //prva.X = vektor[0].X;
            //prva.Y = vektor[0].Y;
            Tocka zadna = new Tocka(vektor[vektor.Count - 1].X, vektor[vektor.Count - 1].Y); // zadna ki prav tako ostane fixna
            //zadna.X = vektor[vektor.Count - 1].X;
            //zadna.Y = vektor[vektor.Count - 1].Y;
            //MessageBox.Show("vecotr count: " + vektor.Count());
            //vektor.CopyTo(temp1);
            for (int i = 1; i < vektor.Count - 1; i++) { temp1.Add(vektor[i]); }
            // sedaj je temp1 kopija vektor brez prve in zadne tocke
            int st_tock = vektor.Count(); // koliko tock imamo
            int st_interakcij = 0; /// koliko interakcij smo naredili
            while (st_tock > stevilo_vektorjev-2) {
                //MessageBox.Show("Stevilo tock: " + st_tock);
                if (st_interakcij % 2 == 0)
                {
                    // iz temp1 kopiramo v temp2
                    st_tock = 0;
                    for (int i = 0; i < temp1.Count() - 1; i = i + 2) {
                        double x = (temp1[i].X + temp1[i + 1].X) / 2; // izracunamo vmesno tocko
                        double y = (temp1[i].Y + temp1[i + 1].Y) / 2;
                        temp2.Add(new Tocka(x, y)); // si jo shranimo
                        st_tock++; // povecamo koliko tock imamo
                    }
                    temp1.Clear(); // pocistimo tempo1 za naslednji krog
                    st_interakcij++; // zaj je st_i%2==1
                }
                else {
                    // iz temp2 kopiramo v temp1
                    st_tock = 0; // damo tocke na 0
                    for (int i = 0; i < temp2.Count() - 1; i = i + 2)
                    {
                        double x = (temp2[i].X + temp2[i + 1].X) / 2;
                        double y = (temp2[i].Y + temp2[i + 1].Y) / 2;
                        temp1.Add(new Tocka(x, y));
                        st_tock++;
                    }
                    temp2.Clear();
                    st_interakcij++; // zaj je st_i%2==0
                }
            }
            vektor_2.Add(prva); // dodamo prvo tocko na prvo mesto
            if (st_interakcij % 2 == 0)
            {
                for (int i = 0; i < temp1.Count(); i++)
                {
                    vektor_2.Add(temp1[i]); // si prekopiramo tocke
                }
            }
            else {
                for (int i = 0; i < temp2.Count(); i++)
                {
                    vektor_2.Add(temp2[i]); // si prekopiramo tocke
                }
            }
            vektor_2.Add(zadna); // dodamo na zadno mesto
            Brush aBrush = (Brush)Brushes.Red; // za risanje tock
            for (int i = 0; i < vektor_2.Count(); i++) {
                g.FillRectangle(aBrush, (int)vektor_2[i].X, (int)vektor_2[i].Y, 4, 4); // narisemo tocko
            }
            temp1.Clear();
            temp2.Clear();

        }

        private void normalizacija() {
            for (int i = 0; i < vektor_2.Count(); i++) {
                Tocke.Add(new Tocka((vektor_2[i].X / maxX), (vektor_2[i].Y / maxY)));
                MessageBox.Show("x: " + Tocke[i].X + " y: " + Tocke[i].Y);
            }
        }
    }

    public class Tocka // moj class za tocko
    {
        public double X, Y;
        public Tocka(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
