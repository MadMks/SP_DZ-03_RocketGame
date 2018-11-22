using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RocketGame
{
    internal class Asteroid : PictureBox
    {
        private const int fallingStep = 1;
        private const int maxAsteroidSize = 20;
        public static int MaxAsteroidSize { get; }

        private MainForm form = null;


        public Asteroid(MainForm form, int x)
        {
            this.BackColor = Color.Black;
            this.Size = new Size(maxAsteroidSize, maxAsteroidSize);
            this.Location = new Point(x, 0 - maxAsteroidSize);

            this.form = form;
            this.form.Invoke(new Action(this.ShowAsteroid));
        }

        private void ShowAsteroid()
        {
            form.Controls.Add(this);
        }

        internal void StartOfTheFall()
        {
            Console.WriteLine("StartOfTheFall");
            while (this.Location.Y != form.ClientSize.Height)
            {
                if (form.GameProcess.IsContinues)
                {
                    Thread.Sleep(fallingStep);
                    form.Invoke(new Action(MoveDown));
                    //this.Location = new Point(this.Location.X, this.Location.Y + 1);

                    //if (this.pictureBoxRocket != null)
                    //{
                    Rectangle rectAsteroid = this.DisplayRectangle;
                    rectAsteroid.Location = this.Location;

                    if (form.GameProcess.Rocket.GetRectRocket()
                        .IntersectsWith(rectAsteroid))
                    {
                        form.GameProcess.StopFallingAsteroids();
                        form.GameProcess.IsContinues = false;


                        //this.RocketFall();

                        //this.MenuDesignSettings();

                        //this.GameOver = true;
                    }
                    //}
                }
            }


            if (this.Location.Y == this.ClientSize.Height)
            {
                Console.WriteLine(">>>>> delete 0");
                form.Invoke(new Action(DeleteTaskAsteroid));
                this.Dispose();  // удаление ненужного (упавшего) астероида.
            }

            Console.WriteLine(form.GameProcess.ListTasksAsteroids.Count);
        }

        private void DeleteTaskAsteroid()
        {
            form.GameProcess.ListTasksAsteroids.RemoveAt(0);
        }

        private void MoveDown()
        {
            this.Location = new Point(this.Location.X, this.Location.Y + 1);
        }
    }
}