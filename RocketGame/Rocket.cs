using System.Drawing;
using System.Windows.Forms;

namespace RocketGame
{
    internal class Rocket : PictureBox
    {
        private const int SIZE_ROCKET_X = 20;
        private const int SIZE_ROCKET_Y = 20;

        public Rocket()
        {
            this.BackColor = Color.Green;
            this.Size = new Size(SIZE_ROCKET_X, SIZE_ROCKET_Y);

        }

    }
}