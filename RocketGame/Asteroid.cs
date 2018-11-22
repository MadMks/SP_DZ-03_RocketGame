using System;
using System.Drawing;
using System.Windows.Forms;

namespace RocketGame
{
    internal class Asteroid : PictureBox
    {
        private const int maxAsteroidSize = 20;
        public static int MaxAsteroidSize { get; }

        public Asteroid(MainForm form, int x)
        {
            this.BackColor = Color.Black;
            this.Size = new Size(maxAsteroidSize, maxAsteroidSize);
            this.Location = new Point(x, 0 - maxAsteroidSize);

            form.Invoke(new Action<MainForm, Asteroid>(this.ShowAsteroid), form, this);
        }

        private void ShowAsteroid(MainForm form, Asteroid asteroid)
        {
            form.Controls.Add(asteroid);
        }
    }
}