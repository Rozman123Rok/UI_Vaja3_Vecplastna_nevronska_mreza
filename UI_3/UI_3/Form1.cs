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
        public Point nova = new Point();
        public Point stara = new Point();
        public Graphics g;
        public Pen pen = new Pen(Color.Black, 5);
        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            stara = e.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
            {
                nova = e.Location;
                g.DrawLine(pen, stara, nova);
                stara = nova;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }
    }
}
