using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
namespace pendulum
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Boolean sampling = false;
        ArrayList tt = new ArrayList();
        ArrayList ww = new ArrayList();
        ArrayList xx = new ArrayList();
        ArrayList aa = new ArrayList();
        int count = 0;
        Boolean isMoving = false;
        double Teta = Math.PI, Omega = 0, Alfa = 0, x = 0, v = 0,v0=0, a = 0, T = 0, m = 0.174, g = 9.8, l = 0.51;
        double V = 0, V0 = 0, W = 0, dW = 0, W0 = 0, h = 30, f = 1.11;
        double teta = 0;
        int x0,Lx;
        Boolean isMouseDown = false;
        
        private void btnStart_Click(object sender, EventArgs e)
        {
              btnStart.Enabled = false;
              btnStop.Enabled = true;
              DateTime t0 = DateTime.Now;
              int dt = 0,time=0;
              isMoving = true;
              double Lastx = x, lastv = v;
              Boolean ismove = false;
              while (isMoving)
              {
                  DateTime t = DateTime.Now;
                  int Milliseconds = (t.Hour - t0.Hour) * 3600000 + (t.Minute - t0.Minute) * 60000 + (t.Second - t0.Second) * 1000 + (t.Millisecond - t0.Millisecond);
                  dt = Milliseconds - time;
                  if (dt == 0) continue;
                  time = Milliseconds;
                  lblTime.Text = time.ToString();
                  Application.DoEvents();
                  Bitmap pic = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                  
                  if (isMouseDown)
                  {
                      v = (x - Lastx) / (dt / 1000.0);
                      a = (v - lastv) / (dt / 1000.0);
                      Lastx = x;
                      lastv = v;
                  }
                  else
                  {
                      //try { V = double.Parse(txtV.Text); }
                      //catch { V = 0; }
                      if (checkBox2.Checked||checkBox1.Checked)
                      {
                          v = v + a * dt / 1000.0;
                          V = (50 * a + f * 50 * v) / h;
                          try
                          {
                              trackBar1.Value = (int)(V * 12);
                          }
                          catch { }
                      }
                      else if(checkBox3.Checked)
                      {
                          a = (v - v0) / (dt / 1000.0);
                          v0 = v;
                          if (W == 0) ismove = false;
                          else ismove = true;

                          if (!ismove && Math.Abs(V0) > 1.56) ismove = true;
                          if(ismove) W = (h * V0 - f * W0 + W0 * 1000.0 / dt) * (dt / 1000.0);
                          if (Math.Abs(W) < 1.5) W = 0;
                          v = W / 50;
                          if (v > 0) v = v - (dt / 1000.0);
                          else if (v < 0) v = v + (dt / 1000.0);
                      }
                      else
                      {
                          V = trackBar1.Value*8/100.0;
                          a = (v - v0) / (dt / 1000.0);
                          v0 = v;
                          if (v == 0 && Math.Abs(V) < 1.2) W = 0;
                          else W = (h * V - f * W0 + W0 * 1000.0 / dt) * (dt / 1000.0);
                          v = W / 50;
                          if (v > 0) v = v - (dt / 1000.0);
                          else if (v < 0) v = v + (dt / 1000.0);
                          if (Math.Abs(v) < 0.1) v = 0;
                      }
                      //v = v + a * dt / 1000.0;
                      //V = (50 * a + f * 50 * v) / h;
                      double k = (x + (v * dt / 1000.0));
                      if (k * 160 < (-pictureBox1.Width / 2) + 110) v = a = 0;
                      else if (k*160 > pictureBox1.Width / 2 - 110) v = a = 0;
                      else x = k;
                      Lastx = x;
                      lastv = v;
                      W0 = W;
                      V0 = V;
                  }
                  
                  T = (-m * a) * Math.Cos(Teta);
                  Alfa = (T - m * g * l * Math.Sin(Teta)) / (m * l * l);
                  Omega = Omega + Alfa * dt/1000.0;
                  Teta = Teta + Omega * dt/1000.0;
                  teta = (Teta - ((int)(Teta / (2 * Math.PI)) * 2 * Math.PI));
                  if (teta > 0)
                  {
                      teta = teta - Math.PI;
                  }
                  else
                  {
                      teta = -(Math.PI - teta);
                  }

                  if (teta < -2 * Math.PI) teta += Math.PI * 2;
                  if (teta < -Math.PI) teta = 2 * Math.PI + teta;
                  if (teta > Math.PI) teta = (-2 * Math.PI + teta);

                  if (checkBox1.Checked) GetIt();
                  if (checkBox2.Checked) SpeedIt();

                  //گاری
                  for (int xx = -24; xx < 25; xx++)
                  {
                      for (int yy = -2; yy < 4; yy++)
                      {
                          pic.SetPixel((int)(x*160 + pictureBox1.Width / 2 + xx), (int)(pictureBox1.Height / 2 + yy),isMouseDown? Color.DarkOrchid:Color.LightCyan);
                      }
                  }
                  //میله
                  for (int r = 0; r < l*240/1.5; r++)
                  {
                      try
                      {
                          pic.SetPixel((int)(pictureBox1.Width / 2 + x * 160 + r * Math.Sin(Teta)), (int)(pictureBox1.Height / 2 + r * Math.Cos(Teta)), Color.White);
                      }catch{ }
                  }
                  //نقطه ها
                  for (double z = 0; z < Math.PI * 2; z += Math.PI / 12)
                  {
                      for (int r = 0; r < 3; r++)
                      {
                          pic.SetPixel((int)(x * 160 + pictureBox1.Width / 2 + r * Math.Sin(z)), (int)(pictureBox1.Height / 2 + r * Math.Cos(z)), Color.Blue);
                      }
                      for (int r = 0; r < 1 + 7 * Math.Pow(m, 0.333); r++)
                      {
                          try
                          {
                              pic.SetPixel((int)(pictureBox1.Width / 2 + x * 160 + l * 240 / 1.5 * Math.Sin(Teta) + r * Math.Sin(z)), (int)(pictureBox1.Height / 2 + l * 240 / 1.5 * Math.Cos(Teta) + r * Math.Cos(z)), Color.Yellow);
                          }
                          catch { }
                      }
                  }
                  pictureBox1.Image = pic;
              }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isMoving = false;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isMoving = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x0 = e.X;
            Lx = (int)(x*160);
            isMouseDown = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                int k = e.X - x0 + Lx;
                if (k < (-pictureBox1.Width / 2) + 110) k = (-pictureBox1.Width / 2) + 110;
                else if (k > pictureBox1.Width/2 - 110) k = pictureBox1.Width/2 - 110;
                x = k / 160.0;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        void GetIt()
        {
            double k = Omega * 10 * Math.Exp(-Math.Abs(Alfa)) / Math.Cos(Teta);
            if (Math.Abs(k) > 100) k = Math.Sign(k) * 100;
            a = k;
            if (Math.Abs(Alfa) < 0.1 && Math.Abs(Omega) < 0.1) v /= 1.01;
            label3.Text = "Omega : " + ((int) Omega) . ToString() + " rad/s";
        }
        double asd = 0;
        void SpeedIt()
        {
            double buf = -10 * (teta + Omega/1.2-v/7);
            if (buf < -3) a = -3;
            else if (buf > 3) a =3;
            else a =buf;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = "teta     : " + teta + "\n" + "voltage: " + V + "\n" + "a         : " + a.ToString() + "\nx         : " + x.ToString() + "\nw         : " + (v*50).ToString(); ; ;
            if (sampling)
            {
                tt.Add(teta);
                ww.Add(V);
                xx.Add(v);
                aa.Add(a);
                count = tt.Count;
                lblSamples.Text = count.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Teta = Teta + 0.25;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Teta = Teta - 0.1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double ll;
                ll = double.Parse(textBox1.Text);
                if (ll > 0) l = ll;
            }
            catch { }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double ll;
                ll = double.Parse(textBox2.Text);
                if (ll > 0) m = ll;
            }
            catch { }
        }

        private void btnStartSampling_Click(object sender, EventArgs e)
        {
            if (btnStartSampling.Text == "Start Sampling")
            {
                tt.Clear();
                ww.Clear();
                aa.Clear();
                xx.Clear();
                count = 0;
                sampling = true;
                btnStartSampling.Text = "Stop Sampling";
            }
            else if (btnStartSampling.Text == "Stop Sampling")
            {
                sampling = false;
                Charts ch = new Charts();
                ch.Chart(tt, ww, xx, aa);
                btnStartSampling.Text = "Start Sampling";
                ch.Show();
            }
        }

   

        double va = 0;
        double integral = 0;
        double axel = 0,vlast=0,error,diffrent=0,errorlast=0,aref=0.02,integral2=0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            aref = (-10 * (teta + Omega / 1.2 - v / 9));
            error = aref - a;
            diffrent = (error - errorlast) / (timer2.Interval / 1000.0);
            errorlast = error;
            integral = integral + error * timer2.Interval / 1000.0;
            integral = integral * 0.95;

            va=(va*14 +  4*(error + integral))/15.0;
            if (va > 8) va = 8;
            else if (va < -8) va = -8;
            //if (Math.Sign(va) == -Math.Sign(V)) v = 0;
            V = va;

            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            timer2.Enabled = checkBox3.Checked;
        }

        private void txtV_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
