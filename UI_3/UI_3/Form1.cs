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
        public float stopnja_ucenja = 0.25f; // stopnja ucenja
        public float dovoljena_napaka = 0.005f; // napaka
        int maxX = 587; // velikost panel  na kateri risem
        int maxY = 426; // velikost panel  na kateri risem
        string Znacka_znaka; // kateri znak smo vpisali
        public List<Tocka> nor_Tocke = new List<Tocka>(); // tu imamo shranjene vse tocke
        public List<Znak> znaki = new List<Znak>();
        NeuralNetwork mreza = new NeuralNetwork(new int[] { 16 * 2, 12 , 10 });

        //public NeuralNetwork mreza = new NeuralNetwork(new int[] { stevilo_vektorjev * 2, stevilo_nevronov, 10 });

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
            float x = (float)stara.X;
            float y = (float)stara.Y;
            vektor.Add(new Tocka(x, y)); // dodamo v vektor
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // ce drzimo levi gumb na miski
            {
                nova = e.Location; // za vsak premik dobimo novo lokavijo
                float x = (float)nova.X;
                float y = (float)nova.Y;
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
            /*
            if (poteka_ucenje) // ce poteka ucenje
            {*/
                // moramo vpisati kateri znak smo narisali
                //MessageBox.Show("Tukaj bos vpisal keri znak je");
                Znacka_znaka = Prompt.ShowDialog("Vpisi znacko narisanega znaka");
                Vektorizacija();
                //vektorizacija();
                normalizacija();
                MessageBox.Show("Vpisan text: " + Znacka_znaka);
                //MessageBox.Show("Stevilo v vektor2: " + vektor_2.Count());
/*
            }
            else {
                Vektorizacija();
                normalizacija();
                //MessageBox.Show("Output: " + mreza.FeedForward(new float[] { 0, 0, 0 })[0]);
            }*/
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
            stopnja_ucenja = (float)numericUpDown3.Value; // pridobimo vrednost
            //MessageBox.Show("St ucenja: " + stopnja_ucenja);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Napaka: " + dovoljena_napaka);
            dovoljena_napaka = (float)numericUpDown4.Value; // pridobimo vrednost
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
                        float x = (temp1[i].X + temp1[i + 1].X) / 2; // izracunamo vmesno tocko
                        float y = (temp1[i].Y + temp1[i + 1].Y) / 2;
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
                        float x = (temp2[i].X + temp2[i + 1].X) / 2;
                        float y = (temp2[i].Y + temp2[i + 1].Y) / 2;
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
            List<Tocka> temp2 = new List<Tocka>(); // vmesne tocke
            // si shranim prvo pa zadno tocko ker ostaneta fix
            Tocka prva = new Tocka(vektor[0].X, vektor[0].Y); // prva tocka ki ostane fix
            Tocka zadna = new Tocka(vektor[vektor.Count - 1].X, vektor[vektor.Count - 1].Y); // zadna ki prav tako ostane fixna

            bool na_vrsti_temp1 = true; // kdaj je vektor ena na vrsti
            // prekopiramo vse tocke v temp1 razen prve pa zadne
            for (int i = 1; i < vektor.Count - 1; i++) { temp1.Add(vektor[i]); } /// kopiram vse tocke v vektor1

            bool morem_b = false; // morem koncat 

            int stevilo_tock = temp1.Count(); // koliko tock imamo na zacetku (to so vse -2)
            //MessageBox.Show("St_tock: " + stevilo_tock + " st_vek: " + (stevilo_vektorjev - 2));
            // dokler ni stevilo tock vecje kot 2xstevilo dovoljenih tock
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
                            float x = (temp1[i].X + temp1[i + 1].X) / 2; // izracunamo vmesno tocko
                            float y = (temp1[i].Y + temp1[i + 1].Y) / 2;
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
                            float x = (temp2[i].X + temp2[i + 1].X) / 2; // izracunamo vmesno tocko
                            float y = (temp2[i].Y + temp2[i + 1].Y) / 2;
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
            // preverimo koliko tock je prevec ce jih je kaj
            int prevec;
            int odstranit;
            if (na_vrsti_temp1) {
                prevec = temp1.Count() - (stevilo_vektorjev - 2);
                if (prevec != 0) { 
                odstranit = temp1.Count() / prevec;
                for (int i = odstranit - 1; i < temp1.Count(); i = i + odstranit) {
                    temp1.RemoveAt(i);
                }
            }
            }
            else { 
                prevec = temp2.Count() - (stevilo_vektorjev-2);
                if (prevec != 0)
                {
                    odstranit = temp2.Count() / prevec;
                    for (int i = odstranit - 1; i < temp2.Count(); i = i + odstranit)
                    {
                        temp2.RemoveAt(i);
                    }
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

            // narisemo tocke
            Brush aBrush = (Brush)Brushes.Red; // za risanje tock
            for (int i = 0; i < vektor_2.Count(); i++)
            {
                g.FillRectangle(aBrush, (int)vektor_2[i].X, (int)vektor_2[i].Y, 4, 4); // narisemo tocko
            }
            temp1.Clear(); // pocistimo temp1
            temp2.Clear(); // pocistimo temp2
            /// da jih lahko naslednic ponovno uporabimo
            /// 
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

            if (poteka_ucenje)
            {
                // torej jih se bomo dali skozi mrezo
                MessageBox.Show("Sedaj bom začel z učenjem!");
                //tester(); /// to zaj dela zaj pa se mores ugotovit kak spravit noter vektorje
                //NeuralNetwork mreza = new NeuralNetwork(new int[] { stevilo_vektorjev * 2, stevilo_nevronov, 10 });
                //for (int z = 0; z < 100; z++)
                //{
                    for (int i = 0; i < znaki.Count(); i++)
                    {
                        float[] temp = new float[znaki[i].tocke_znaka.Count() * 2]; // temp ki ga bomo posilali v mrezo, mora biti 2* koliko tock je (X in Y)
                        int st = 0;
                        //MessageBox.Show("Znak: " + znaki[i].oznaka + " st tock: " + znaki[i].tocke_znaka.Count());
                        for (int j = 0; j < znaki[i].tocke_znaka.Count(); j++)
                        {
                            temp[st] = znaki[i].tocke_znaka[j].X; // kopiramo X
                            temp[st + 1] = znaki[i].tocke_znaka[j].Y; // kopiramo Y
                            st += 2;
                        }
                        if (temp.Length != stevilo_vektorjev * 2)
                        {
                            //MessageBox.Show("Skipam " + temp.Length + " " + znaki[i].oznaka);
                        }
                        else
                        {
                            int numVal = Int32.Parse(znaki[i].oznaka); // si oznako damo v int
                                                                       //MessageBox.Show("Naprej poslal: " + numVal + " st_tock: " + temp.Length + " dovoljeno: " + stevilo_vektorjev * 2);
                            mreza.FeedForward(temp);
                            float[] nekaj = new float[10];
                            for (int j = 0; j < 10; j++)
                            {
                                if (numVal == j)
                                {
                                    nekaj[j] = 1; // na tistem mestu ko je oznaka damo na 1
                                }
                                else
                                {
                                    nekaj[j] = 0; // ostale damo na 0
                                }
                            }

                            mreza.Backprop(nekaj); // in pa vrnemo pricakovan izhod da se uredijo utezi
                        }
                    }

                //}
                MessageBox.Show("Koncal s ucenjem!");
            }
            else {
                // smo pripravljeni na ugotavljanje
                float[] temp = new float[znaki[znaki.Count() - 1].tocke_znaka.Count() * 2]; // damo koliko tock bomo poslali not
                int st = 0;
                for (int i = 0; i < znaki[znaki.Count() - 1].tocke_znaka.Count(); i++) {
                    // prekopiramo tocke
                    temp[st] = znaki[znaki.Count() - 1].tocke_znaka[i].X;
                    st++;
                    temp[st] = znaki[znaki.Count() - 1].tocke_znaka[i].Y;
                    st++;
                }
                //int numVal = Int32.Parse(znaki[znaki.Count() - 1].oznaka);
                //MessageBox.Show("Temp: " + temp.Length + " st_tock: " + znaki[znaki.Count() - 1].tocke_znaka.Count());
                //MessageBox.Show("Output: " + mreza.FeedForward(temp)[0] + " znak: " + znaki[znaki.Count() - 1].oznaka);
                float max = -999;
                float value;
                int index = 0;
                for (int i = 0; i < mreza.FeedForward(temp).Length; i++) {
                    value = mreza.FeedForward(temp)[i];
                    //MessageBox.Show("Output: " + value + " znak: " + znaki[znaki.Count() - 1].oznaka);
                    if (value > max) { max = value;  index = i; } // si shranimo index max value
                }
                MessageBox.Show("Zmagal index: " + index); // to bi morala biti nasa resitev oz oznaka
            }

        }

        public void tester()
        {
            // test za mrezo ce deluje
            // na primeru xor 
            // DELUJE
            NeuralNetwork net = new NeuralNetwork(new int[] { 3, 25, 25, 1 }); // nastavimo mrezo

            for (int i = 0; i < 5000; i++)
            {

                net.FeedForward(new float[] { 0, 0, 0 }); // poslemo not array
                net.Backprop(new float[] { 0 }); // poslemo pricakovan izhod

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

            MessageBox.Show("Output(0): " + net.FeedForward(new float[] { 0, 0, 0 })[0]); // tu stestiramo ce prav napise
            MessageBox.Show("Output(1): " + net.FeedForward(new float[] { 0, 0, 1 })[0]);
            MessageBox.Show("Output(1): " + net.FeedForward(new float[] { 1, 1, 1 })[0]);
            MessageBox.Show("Output(0): " + net.FeedForward(new float[] { 0, 1, 1 })[0]);

        }
    }

    public class Tocka // moj class za tocko
    {
        public float X, Y;
        public Tocka(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    // za okno da vpisem not kateri znak sem narisal
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
        public string oznaka; // oznaka znaka kaj smo narisali
        public List<Tocka> tocke_znaka = new List<Tocka>(); // shranimo tocke
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
        int[] layer; // da lahko imamo koliko zelimo veliko mrezo oz koliko zelimo layer
        Layer[] layers; // dejanska mreza
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

                layers[i] = new Layer(layer[i], layer[i + 1]); // damo not stevilo vhodov in izhodov
            }
        }

        public float[] FeedForward(float[] vhod)
        {
            layers[0].feedForward(vhod); // v prvi layer damo nas input to so tocke
            for (int i = 1; i < layers.Length; i++)
            {
                layers[i].feedForward(layers[i - 1].izhodi); // potem dajemo naprej izhode od prejsnih
            }
            return layers[layers.Length - 1].izhodi; // vrnemoi koncni izhod
        }

        public void Backprop(float[] pricakovano) {
            for (int i = layers.Length - 1; i >= 0; i--) {
                if (i == layers.Length - 1)
                {
                    layers[i].BackpropOutput(pricakovano); // smo na zadnem layer oz output layer in mu damo noter samo to kar smo pricakovali
                }
                else {
                    layers[i].BackpropHidden(layers[i+1].gama, layers[i+1].utezi); // za ostale vmesne layere
                }
            }

            for (int i = 0; i < layers.Length; i++) {
                layers[i].PosodobiUtezi(); // gremo skozi vse layere in posodobimo utezi
            }
        }

    }
    public class Layer
        {
            int st_vhodov; // koliko nevronov v prejsni layer
            int st_izhodov; // koliko je nevronov v tem layer

            public float[] izhodi; // izhod ki ga da nevron
            public float[] vhodi; // vhod ki ga prejme
            public float[,] utezi; // utezi
            public float[,] uteziDelta; // kako bomo posodobili utezi
            public float[] gama;
            public float[] napaka; // za koliko smo se zmotili
            public static Random rand = new Random();
            public float stopnja_ucenja = 0.25f; // stopnja ucenja

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
                        izhodi[i] += vhodi[j] * utezi[i, j]; // na izhod poslemo vhode pomnozene s utezmi in sestete
                    }
                    izhodi[i] = (float)Math.Tanh(izhodi[i]); // Hyperbolic tangent
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
                        utezi[i, j] -= uteziDelta[i, j] * stopnja_ucenja; // posodobimo utezi da jim odstejemo delto pomnozeno s st_ucenja
                    }
                }
            }


        public void BackpropOutput(float[] pricakovano) { 
            /// backprop za output layer to so zadnje layer
            for(int i=0; i < st_izhodov; i++)
            {
                napaka[i] = izhodi[i] - pricakovano[i]; // od izhoda odstejemo pricakovano
            }

            for (int i = 0; i < st_izhodov; i++) {
                gama[i] = napaka[i] * TahnDer(izhodi[i]); // dobimo gamo za izracunanje delte
            }

            for (int i = 0; i < st_izhodov; i++) { 
                for(int j=0; j < st_vhodov; j++)
                {
                    uteziDelta[i, j] = gama[i] * vhodi[j]; // izracunamo delto

                }
            }
        }

        public float TahnDer(float value)
        {
            return 1 - (value * value); // 1/x2
        }

        public void BackpropHidden(float[] gamaForward, float[,]forward) {
            /// backprop za skrite layere
            for (int i = 0; i < st_izhodov; i++)
            {
                gama[i] = 0;
                for (int j = 0; j < gamaForward.Length; j++) {
                    gama[i] += gamaForward[j] * forward[j, i]; // gami pristejemo gamo ki smo jo dobili pomnozeno z vrednostjo za naprej
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

}
