using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aircounter
{
    public partial class Aircounter : Form
    {
        private int z2 = 1;
        private int z3 = 2;
        private int _x;
        private int _y;
        private int _x2;
        private int _y2;
        private int _x3;
        private int _y3;
        private bool simfinished;
        private int airplanecount = 0;
        private int clicks = 0;
        private static readonly Random getrandom = new Random();

        // Funkcja zapewniająca liczby pseudolosowe
        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) 
            {
                return getrandom.Next(min, max);
            }
        }

        // Utworzenie tablic przechowujących dane obiektów
        Airplane[] airplanes = new Airplane[20];
        Bitmap[] bitmapArray = new Bitmap[20];

        public Aircounter()
        {
            // Wygenerowanie bazy dwudziestu losowych samolotów biorących udział w symulacji
            for (int i = 0; i < 20; i++)
            {
                airplanes[i] = new Airplane()
                {
                    AirplaneSize = GetRandomNumber(2, 5),
                    AirplaneTrajectory = GetRandomNumber(-2, 3),
                    AirplaneColor = GetRandomNumber(1, 6)
                };

                // Przypisanie bitmapy w zależności od parametru koloru 
                switch(airplanes[i].AirplaneColor)
                {
                    case 1:
                        bitmapArray[i] = new Bitmap("plane_black.png");
                        break;
                    case 2:
                        bitmapArray[i] = new Bitmap("plane_blue.png");
                        break;
                    case 3:
                        bitmapArray[i] = new Bitmap("plane_green.png");
                        break;
                    case 4:
                        bitmapArray[i] = new Bitmap("plane_red.png");
                        break;
                    case 5:
                        bitmapArray[i] = new Bitmap("plane_yellow.png");
                        break;
                }
            }

            // Sprawdzenie liczby samolotów odpowiadających obiektowi początkowemu
            for (int i = 1; i < 20; i++)
            {
                if((airplanes[0].AirplaneSize == airplanes[i].AirplaneSize)&&(airplanes[0].AirplaneColor == airplanes[i].AirplaneColor))
                {
                    airplanecount++; 
                }
            }


            // Ustawienie parametrów symulacji
            _x = -150;
            _y = 170;
            _x2 = -100;
            _y2 = 170;
            _x3 = -550;
            _y3 = 140;
            simfinished = false;

            

            InitializeComponent();
        }

        private void Aircounter_Paint(object sender, PaintEventArgs e)
        {
            
            // Ustalenie warunków symulacji w zależności od położenia obiektów
            if (_x < 1000)
            {
                e.Graphics.DrawImage(bitmapArray[0], _x, _y, airplanes[0].AirplaneSize * 25, airplanes[0].AirplaneSize * 25);
            }

            if ((_x2 < 1000)&&(z2 <= 18))
            {
                e.Graphics.DrawImage(bitmapArray[z2], _x2, (_y2 + 60 * airplanes[z2].AirplaneTrajectory), airplanes[z2].AirplaneSize * 25, airplanes[z2].AirplaneSize * 25);
            }

            if ((_x3 < 1000)&&(z3 <= 19))
            {
                e.Graphics.DrawImage(bitmapArray[z3], _x3, (_y3 + 60 * airplanes[z3].AirplaneTrajectory), airplanes[z3].AirplaneSize * 25, airplanes[z3].AirplaneSize * 25);
            }

            if (_x2 >= 1000)
            {
                _x2 = -50;
                z2 = z2 + 2;
    
            }

            if (_x3 >= 1000)
            {
                _x3 = -50;
                z3 = z3 + 2;
            }

            if (z3 > 19)
            {
                simfinished = true;
            }

            if (simfinished)
            {
                string message; 

                if(airplanecount == clicks)
                {
                    message = "Gratulację! Udało się podliczyć samoloty prawidłowo!";
                    SolidBrush s = new SolidBrush(Color.Green);
                    Graphics g = CreateGraphics();
                    FontFamily ff = new FontFamily("Courier New");
                    System.Drawing.Font font = new System.Drawing.Font(ff, 20, FontStyle.Regular);
                    g.DrawString(message, font, s, new PointF(20, 100));
                }

                else
                {
                    message = "Niestety, nie udało się podliczyć samolotów. Spróbuj jeszcze raz!";
                    SolidBrush s = new SolidBrush(Color.Red);
                    Graphics g = CreateGraphics();
                    FontFamily ff = new FontFamily("Courier New");
                    System.Drawing.Font font = new System.Drawing.Font(ff, 15, FontStyle.Regular);
                    g.DrawString(message, font, s, new PointF(20, 100));
                }
            }

        }

        

        private void timerStartingLoop_Tick(object sender, EventArgs e)
        {

            // Zegar odpowiadający za poruszanie się obiektów
            if (_x == 470)
            {
                timerStartingLoop.Stop();
                System.Threading.Thread.Sleep(4000);
                timerStartingLoop.Start();
            }

            if (_x < 1200)
            {
                _x += 20;
            }

            else
            {
                _x2 += 25;
                _x3 += 25;
            }

           

            Invalidate();
        }

        private void Aircounter_Click(object sender, EventArgs e)
        {

           /* double f = airplanecount;
            string sss = f.ToString("R");
            SolidBrush s = new SolidBrush(Color.Yellow);
            Graphics g = CreateGraphics();
            FontFamily ff = new FontFamily("Courier New");
            System.Drawing.Font font = new System.Drawing.Font(ff, 50, FontStyle.Regular);
            g.DrawString( sss, font, s, new PointF(20, 20)); */
            
        }

        // Prosta animacja i funkcjonalności przycisków dostępnych w ramach gameplayu
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.newbutton2;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.newbutton1;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            clicks++;
            string checkclicks = clicks.ToString();
            label1.Text = checkclicks;
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.repeat2;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.repeat1;
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.escape2;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.escape1;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
