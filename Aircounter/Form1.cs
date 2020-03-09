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
    /// <summary>
    /// Klasa dziedzicząca po całym oknie Form
    /// </summary>
    public partial class Aircounter : Form
    {
        /// <summary>
        /// Zmienna określajaca index nieparzystych samolotów
        /// </summary>
        private int z2 = 1;
        /// <summary>
        /// Zmienna określająca index parzystych samolotów
        /// </summary>
        private int z3 = 2;
        /// <summary>
        /// Współrzędna x położenia obiektu początkowego
        /// </summary>
        private int _x;
        /// <summary>
        /// Współrzędna y położenia obiektu początkowego
        /// </summary>
        private int _y;
        /// <summary>
        /// Współrzędna x położenia nieparzystych zamolotów
        /// </summary>
        private int _x2;
        /// <summary>
        /// Współrzędna y położenia nieparzystych samolotów
        /// </summary>
        private int _y2;
        /// <summary>
        /// Współrzędna x położenia parzystych samolotów
        /// </summary>
        private int _x3;
        /// <summary>
        /// Współrzędna y położenia parzystych samolotów
        /// </summary>
        private int _y3;
        /// <summary>
        /// Czy symulacja została zakończona
        /// </summary>
        private bool simfinished;
        /// <summary>
        /// Liczba samolotów odpowiadających obiektowi początkowemu
        /// </summary>
        private int airplanecount = 0;
        /// <summary>
        /// Liczba podliczonych przez użytkownika samolotów
        /// </summary>
        private int clicks = 0;
        private static readonly Random getrandom = new Random();

        /// <summary> 
        /// Funkcja zapewniająca liczby pseudolosowe
        /// </summary>
        /// <param name="max">Maksymalna wartość liczby pseudolosowej</param>
        /// <param name="min">Minimalna wartość liczby pseudolosowej</param>
        /// <returns>
        /// Zwraca liczbę losowa z zakresu min, max
        /// </returns>
        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom)
            {
                return getrandom.Next(min, max);
            }
        }
        /// <summary>
        /// Tablica przechowująca wszystkie samoloty
        /// </summary>
        Airplane[] airplanes = new Airplane[20];
        /// <summary>
        /// Tablica przechowująca wygląd wszystkich samolotów
        /// </summary>
        Bitmap[] bitmapArray = new Bitmap[20];

        /// <summary>
        /// Kontstruktor, w którym generowane są elementy rozgrywki
        /// </summary>
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
                switch (airplanes[i].AirplaneColor)
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
                if ((airplanes[0].AirplaneSize == airplanes[i].AirplaneSize) && (airplanes[0].AirplaneColor == airplanes[i].AirplaneColor))
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

        /// <summary>
        /// Metoda, w której wykonywane jest rysowanie wcześniej wygenerowanych obiektów
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
        private void Aircounter_Paint(object sender, PaintEventArgs e)
        {

            // Ustalenie warunków symulacji w zależności od położenia obiektów
            if (_x < 1050)
            {
                e.Graphics.DrawImage(bitmapArray[0], _x, _y, airplanes[0].AirplaneSize * 25, airplanes[0].AirplaneSize * 25);
            }

            if ((_x2 < 1050) && (z2 <= 18))
            {
                e.Graphics.DrawImage(bitmapArray[z2], _x2, (_y2 + 60 * airplanes[z2].AirplaneTrajectory), airplanes[z2].AirplaneSize * 25, airplanes[z2].AirplaneSize * 25);
            }

            if ((_x3 < 1050) && (z3 <= 19))
            {
                e.Graphics.DrawImage(bitmapArray[z3], _x3, (_y3 + 60 * airplanes[z3].AirplaneTrajectory), airplanes[z3].AirplaneSize * 25, airplanes[z3].AirplaneSize * 25);
            }

            if (_x2 >= 1050)
            {
                _x2 = -50;
                z2 = z2 + 2;

            }

            if (_x3 >= 1050)
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

                if (airplanecount == clicks)
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


        /// <summary>
        /// Zegar, dla którego wykonywana jest cała pętla gry
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
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

        /// <summary>
        /// Metoda odpowiadająca za podświetlenie przycisku
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
        // Prosta animacja i funkcjonalności przycisków dostępnych w ramach gameplayu
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.newbutton2;
        }
        /// <summary>
        /// Metoda odpowiadająca za zakończenie podświetlenia przycisku
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.newbutton1;
        }
        /// <summary>
        /// Metoda odpowiadająca za wysyłanie liczby kliknięć do labelu1
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            clicks++;
            string checkclicks = clicks.ToString();
            label1.Text = checkclicks;
        }
        /// <summary>
        /// Metoda odpowiadająca za podświetlenie przycisku powtórzenia rozgrywki
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.repeat2;
        }
        /// <summary>
        /// Metoda odpowiadająca za zakończenie podświetlania przycisku powtórzenia rozgrywki
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.repeat1;
        }
        /// <summary>
        /// Metoda odpowiadająca za podświetlenie przycisku wyjścia z rozgrywki
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.escape2;
        }
        /// <summary>
        /// Metoda odpowiadająca za zakończenie podświetlania przycisku wyjścia z rozgrywki
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.escape1;
        }
        /// <summary>
        /// Metoda odpowiadająca za wyjście z rozgrywki po kliknięciu przycisku wyjścia
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// Metoda odpowiadająca za powtórzenie rozgrywki po kliknięciu przycisku powtórzenia rozgrywki
        /// </summary>
        /// <param name="sender">Parametr obiektu</param>
        /// <param name="e">Parametr wydarzenia</param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}

