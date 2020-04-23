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
        string Znacka_znaka; // kateri znak smo vpisali
        public List<Tocka> nor_Tocke = new List<Tocka>(); // tu imamo shranjene vse tocke
        public List<Znak> znaki = new List<Znak>();

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
                vektor.Add(new Tocka(x, y)); // dodamo v vektor
                g.DrawLine(pen, stara, nova); // narisemo crto med novo in staro
                stara = nova; // si prepisemo staro
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Invalidate(); // pocistimo panel
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            // ko spustimo misko

            if (poteka_ucenje) // ce poteka ucenje
            {
                // moramo vpisati kateri znak smo narisali
                //MessageBox.Show("Tukaj bos vpisal keri znak je");
                Znacka_znaka = Prompt.ShowDialog("Vpisi znacko narisanega znaka");
                Vektorizacija();
                //vektorizacija();
                normalizacija();
                MessageBox.Show("Vpisan text: " + Znacka_znaka);
                //MessageBox.Show("Stevilo v vektor2: " + vektor_2.Count());

            }

            znaki.Add(new Znak(Znacka_znaka, vektor_2));
            vektor.Clear();
            vektor_2.Clear();
            /*for (int i = 0; i < znaki.Count(); i++) {
                znaki[i].Izpis();
            }*/
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
            //vektor_2.Add(vektor[0]); // prvi element ostane fix
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
            while (st_tock > stevilo_vektorjev - 2) {
                //MessageBox.Show("Stevilo tock: " + st_tock);
                Random rnd = new Random();
                if (st_interakcij % 2 == 0)
                {
                    // iz temp1 kopiramo v temp2
                    st_tock = 0;
                    for (int i = 0; i < temp1.Count() - 1; i = i + 2) {
                        //if (st_tock == stevilo_vektorjev - 2) { break; }
                        double x = (temp1[i].X + temp1[i + 1].X) / 2; // izracunamo vmesno tocko
                        double y = (temp1[i].Y + temp1[i + 1].Y) / 2;
                        temp2.Add(new Tocka(x, y)); // si jo shranimo
                        st_tock++; // povecamo koliko tock imamo
                    }
                    if (st_tock < stevilo_vektorjev - 2) {
                        //MessageBox.Show("Premalo tock!");
                        // dodaj nazaj par tock
                        for (int i = st_tock; i < stevilo_vektorjev; i++)
                        {
                            int index = rnd.Next(0, temp1.Count());
                            temp2.Add(temp1[index]);
                            temp1.RemoveAt(index);

                        }
                    }
                    temp1.Clear(); // pocistimo tempo1 za naslednji krog
                    st_interakcij++; // zaj je st_i%2==1
                }
                else {
                    // iz temp2 kopiramo v temp1
                    st_tock = 0; // damo tocke na 0
                    for (int i = 0; i < temp2.Count() - 1; i = i + 2)
                    {
                        //if (st_tock == stevilo_vektorjev - 2) { break; }
                        double x = (temp2[i].X + temp2[i + 1].X) / 2;
                        double y = (temp2[i].Y + temp2[i + 1].Y) / 2;
                        temp1.Add(new Tocka(x, y));
                        st_tock++;
                    }
                    if (st_tock < stevilo_vektorjev - 2)
                    {
                        //MessageBox.Show("Premalo tock!");
                        // dodaj nazaj par tock
                        for (int i = st_tock; i < stevilo_vektorjev - 2; i++)
                        {
                            int index = rnd.Next(0, temp1.Count());
                            temp1.Add(temp2[index]);
                            temp2.RemoveAt(index);

                        }
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

        private void Vektorizacija() {
            List<Tocka> temp1 = new List<Tocka>(); // shranjene vmesne tocke
            List<Tocka> temp2 = new List<Tocka>();
            
            Tocka prva = new Tocka(vektor[0].X, vektor[0].Y); // prva tocka ki ostane fix
            Tocka zadna = new Tocka(vektor[vektor.Count - 1].X, vektor[vektor.Count - 1].Y); // zadna ki prav tako ostane fixna

            bool na_vrsti_temp1 = true; // kdaj je vektor ena na vrsti
            for (int i = 1; i < vektor.Count - 1; i++) { temp1.Add(vektor[i]); } /// kopiram vse tocke v vektor1
            bool morem_b = false;
            int stevilo_tock = temp1.Count();
            //MessageBox.Show("St_tock: " + stevilo_tock + " st_vek: " + (stevilo_vektorjev - 2));

            while (stevilo_tock > (stevilo_vektorjev-2)*2 && !morem_b) {
                // ker moremo naredit brez prve in zadne
                //MessageBox.Show("st_tock: " + stevilo_tock + " temp1: " + temp1.Count() + " temp2: " + temp2.Count() + "Na vrsti: " + na_vrsti_temp1);
                if (na_vrsti_temp1)
                {
                    // kopiramo iz temp1
                    for (int i = 0; i < temp1.Count() - 1; i = i + 2)
                    {
                        if (morem_b)
                        {
                            temp2.Add(temp1[i]);
                        }
                        else
                        {
                            double x = (temp1[i].X + temp1[i + 1].X) / 2; // izracunamo vmesno tocko
                            double y = (temp1[i].Y + temp1[i + 1].Y) / 2;
                            temp2.Add(new Tocka(x, y)); // si jo shranimo
                            stevilo_tock--; // zmansamo stevilo tock
                        }
                        if (stevilo_tock == stevilo_vektorjev - 2) {
                            //MessageBox.Show("BREAK St_tock: " + stevilo_tock + " st_vek: " + (stevilo_vektorjev - 2) + " temp2: " + temp2.Count());
                            morem_b = true;
                            //break; 
                        }
                    }

                    temp1.Clear(); // pocistimo tempo1 za naslednji krog
                    na_vrsti_temp1 = false;
                    stevilo_tock = temp2.Count();
                }
                else {
                    // kopiramo iz temp2
                    for (int i = 0; i < temp2.Count() - 1; i = i + 2)
                    {
                        if (morem_b)
                        {
                            temp1.Add(temp2[i]);
                        }
                        else
                        {
                            double x = (temp2[i].X + temp2[i + 1].X) / 2; // izracunamo vmesno tocko
                            double y = (temp2[i].Y + temp2[i + 1].Y) / 2;
                            temp1.Add(new Tocka(x, y)); // si jo shranimo
                            stevilo_tock--; // zmansamo stevilo tock
                        }
                        if (stevilo_tock == stevilo_vektorjev - 2) {
                            //MessageBox.Show("BREAK St_tock: " + stevilo_tock + " st_vek: " + (stevilo_vektorjev - 2) + " temp1: " + temp1.Count());
                            morem_b = true;
                            break; }
                    }

                    temp2.Clear(); // pocistimo tempo1 za naslednji krog
                    na_vrsti_temp1 = true;
                    stevilo_tock = temp1.Count();
                }
            }
            int prevec;
            int odstranit;
            if (na_vrsti_temp1) { 
                prevec = temp1.Count() - (stevilo_vektorjev-2);
                odstranit = temp1.Count() / prevec;
                for (int i = odstranit-1; i < temp1.Count(); i = i + odstranit) {
                    temp1.RemoveAt(i);
                }
            }
            else { 
                prevec = temp2.Count() - (stevilo_vektorjev-2);
                odstranit = temp2.Count() / prevec;
                for (int i = odstranit-1; i < temp2.Count(); i = i + odstranit)
                {
                    temp2.RemoveAt(i);
                }
            }
            


            //MessageBox.Show("Manka mi se: " + stevilo_tock + " temp1: " + temp1.Count() + " temp2: " + temp2.Count() + " prevec " + prevec + " odstranit vsako " + odstranit);

            

            vektor_2.Add(prva); // dodamo prvo tocko na prvo mesto
            if (na_vrsti_temp1)
            {
                for (int i = 0; i < temp1.Count(); i++)
                {
                    vektor_2.Add(temp1[i]); // si prekopiramo tocke
                }
            }
            else
            {
                for (int i = 0; i < temp2.Count(); i++)
                {
                    vektor_2.Add(temp2[i]); // si prekopiramo tocke
                }
            }
            vektor_2.Add(zadna); // dodamo na zadno mesto
            Brush aBrush = (Brush)Brushes.Red; // za risanje tock
            for (int i = 0; i < vektor_2.Count(); i++)
            {
                g.FillRectangle(aBrush, (int)vektor_2[i].X, (int)vektor_2[i].Y, 4, 4); // narisemo tocko
            }
            temp1.Clear();
            temp2.Clear();
            //MessageBox.Show("Vektor2 size: " + vektor_2.Count());

        }

        private void normalizacija() {
            for (int i = 0; i < vektor_2.Count(); i++) {
                nor_Tocke.Add(new Tocka((vektor_2[i].X / maxX), (vektor_2[i].Y / maxY)));
                //MessageBox.Show("x: " + Tocke[i].X + " y: " + Tocke[i].Y);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sedaj bom začel z učenjem!");
            tester(); /// to zaj dela zaj pa se mores ugotovit kak spravit noter vektorje
        }

        public void tester()
        {
            NeuralNetwork net = new NeuralNetwork(new int[] { 3, 25, 25, 1 });

            for (int i = 0; i < 5000; i++)
            {

                net.FeedForward(new float[] { 0, 0, 0 });
                net.Backprop(new float[] { 0 });

                net.FeedForward(new float[] { 0, 0, 1 });
                net.Backprop(new float[] { 1 });

                net.FeedForward(new float[] { 0, 1, 0 });
                net.Backprop(new float[] { 1 });

                net.FeedForward(new float[] { 0, 1, 1 });
                net.Backprop(new float[] { 0 });

                net.FeedForward(new float[] { 1, 0, 0 });
                net.Backprop(new float[] { 1 });

                net.FeedForward(new float[] { 1, 0, 1 });
                net.Backprop(new float[] { 0 });

                net.FeedForward(new float[] { 1, 1, 0 });
                net.Backprop(new float[] { 0 });

                net.FeedForward(new float[] { 1, 1, 1 });
                net.Backprop(new float[] { 1 });
            }

            MessageBox.Show("Output(0): " + net.FeedForward(new float[] { 0, 0, 0 })[0]);
            MessageBox.Show("Output(1): " + net.FeedForward(new float[] { 0, 0, 1 })[0]);
            MessageBox.Show("Output(1): " + net.FeedForward(new float[] { 1, 1, 1 })[0]);
            MessageBox.Show("Output(0): " + net.FeedForward(new float[] { 0, 1, 1 })[0]);

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

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption = "")
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
            Button confirmation = new Button() { Text = "Ok", Left = 250, Width = 100, Top = 49, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

    public class Znak {
        public string oznaka;
        public List<Tocka> tocke_znaka = new List<Tocka>();
        public Znak(string x, List<Tocka> y)
        {
            this.oznaka = x;
            //this.tocke_znaka = y;
            for (int i = 0; i < y.Count(); i++) {
                this.tocke_znaka.Add(y[i]);
            }
        }

        public void Izpis() {
            MessageBox.Show("Oznaka znaka: " + this.oznaka + " st tock: " + this.tocke_znaka.Count());
        }
    }

    public class NeuralNetwork
    {
        int[] layer;
        Layer[] layers;
        public NeuralNetwork(int[] layer)
        {
            this.layer = new int[layer.Length];
            for (int i = 0; i < layer.Length; i++)
            {
                this.layer[i] = layer[i];
            }
            layers = new Layer[layer.Length - 1];

            for (int i = 0; i < layers.Length; i++)
            {

                layers[i] = new Layer(layer[i], layer[i + 1]);
            }
        }

        public float[] FeedForward(float[] vhod)
        {
            layers[0].feedForward(vhod);
            for (int i = 1; i < layers.Length; i++)
            {
                layers[i].feedForward(layers[i - 1].izhodi);
            }
            return layers[layers.Length - 1].izhodi;
        }

        public void Backprop(float[] pricakovano) {
            for (int i = layers.Length - 1; i >= 0; i--) {
                if (i == layers.Length - 1)
                {
                    layers[i].BackpropOutput(pricakovano);
                }
                else {
                    layers[i].BackpropHidden(layers[i+1].gama, layers[i+1].utezi);
                }
            }

            for (int i = 0; i < layers.Length; i++) {
                layers[i].PosodobiUtezi();
            }
        }

    }
    public class Layer
        {
            int st_vhodov; // koliko nevronov v prejsni layer
            int st_izhodov; // koliko je nevronov v tem layer

            public float[] izhodi;
            public float[] vhodi;
            public float[,] utezi;
            public float[,] uteziDelta;
            public float[] gama;
            public float[] napaka;
            public static Random rand = new Random();
            public float stopnja_ucenja = 0.25f;

            public Layer(int vhod, int izhod)
            {
                this.st_vhodov = vhod;
                this.st_izhodov = izhod;
                izhodi = new float[st_izhodov];
                vhodi = new float[st_vhodov];
                utezi = new float[st_izhodov, st_vhodov];
                uteziDelta = new float[st_izhodov, st_vhodov];
                gama = new float[st_izhodov];
                napaka = new float[st_izhodov];

                NastaviUtezi();
            }

            public float[] feedForward(float[] vhod)
            {
                this.vhodi = vhod;

                for (int i = 0; i < st_izhodov; i++)
                {
                    izhodi[i] = 0;
                    for (int j = 0; j < st_vhodov; j++)
                    {
                        izhodi[i] += vhodi[j] * utezi[i, j];
                    }
                    izhodi[i] = (float)Math.Tanh(izhodi[i]);
                }

                return izhodi;
            }

            public void NastaviUtezi()
            {
                for (int i = 0; i < st_izhodov; i++)
                {
                    for (int j = 0; j < st_vhodov; j++)
                    {
                        utezi[i, j] = (float)rand.NextDouble() - 0.5f; // dodamo rand vrednost utezi
                    }

                }
            }

            public void PosodobiUtezi()
            {
                for(int i=0; i < st_izhodov; i++)
                {
                    for (int j = 0; j < st_vhodov; j++) {
                        utezi[i, j] -= uteziDelta[i, j] * stopnja_ucenja;
                    }
                }
            }


        public void BackpropOutput(float[] pricakovano) { 
            /// backprop za output layer to so zadnje layer
            for(int i=0; i < st_izhodov; i++)
            {
                napaka[i] = izhodi[i] - pricakovano[i];
            }

            for (int i = 0; i < st_izhodov; i++) {
                gama[i] = napaka[i] * TahnDer(izhodi[i]);
            }

            for (int i = 0; i < st_izhodov; i++) { 
                for(int j=0; j < st_vhodov; j++)
                {
                    uteziDelta[i, j] = gama[i] * vhodi[j];

                }
            }
        }

        public float TahnDer(float value)
        {
            return 1 - (value * value);
        }

        public void BackpropHidden(float[] gamaForward, float[,]forward) {
            /// backprop za skrite layere
            for (int i = 0; i < st_izhodov; i++)
            {
                gama[i] = 0;
                for (int j = 0; j < gamaForward.Length; j++) {
                    gama[i] += gamaForward[j] * forward[j, i];
                }
                gama[i] *= TahnDer(izhodi[i]);
            }
            for (int i = 0; i < st_izhodov; i++)
            {
                for (int j = 0; j < st_vhodov; j++)
                {
                    uteziDelta[i, j] = gama[i] * vhodi[j];

                }
            }
        }
         
    }


    /*
    public class Nevron {
        public double vrednost;
        double utezi;
        
        public Nevron(double v) {
            this.vrednost = v;
            rand_utez();
        }

        public void rand_utez() {
            var rnd = new Random();
            this.utezi = rnd.NextDouble();
        }

        public aktivacijska() { 
            
        }
    }

    public class Mreza {
        double stopnja_ucenja;
        double napaka;
        int stevilo_vektorjev;
        int stevilo_nevronov;
        List<Nevron> nevroni = new List<Nevron>();
        double utez;

        public Mreza(int st_v, int st_n, double u, double n) {
            this.stevilo_vektorjev = st_v;
            this.stevilo_nevronov = st_n;
            this.stopnja_ucenja = u;
            this.napaka = n;
        }


        public void rand_utezi() { 
            
        }
    }
    */
}
