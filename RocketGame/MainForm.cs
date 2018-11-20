using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace RocketGame
{
    public partial class MainForm : Form
    {
        private const int SIZE_ROCKET_X = 20;
        private const int SIZE_ROCKET_Y = 20;

        private const int SIZE_ASTEROID = 20;
        private const int AMOUNT_OF_ASTEROIDS_TASK = 3;
        private List<Task> tasks = null;

        private Random random = null;
        private int fallingSpeed;

        private Timer timer = null;


        // temp
        //private PictureBox pictureBox = null;
        private bool temp = false;

        public MainForm()
        {
            InitializeComponent();

            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.random = new Random();
            this.tasks = new List<Task>();
            this.fallingSpeed = 1;
        }

        private void AddRocket()
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.BackColor = Color.Green;
            pictureBox.Size = new Size(SIZE_ROCKET_X, SIZE_ROCKET_Y);
            pictureBox.Location = this.StartPointRocket();
            pictureBox.Visible = true;

            this.Controls.Add(pictureBox);
        }

        private Point StartPointRocket()
        {
            Point point = new Point();
            point.X = (this.ClientSize.Width / 2) - (SIZE_ROCKET_X / 2);
            point.Y = this.ClientSize.Height - SIZE_ROCKET_Y;
            return point;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // Старт игры.

            // Добавление ракеты.
            this.AddRocket();

            // Старт метода поверки столкновения.
            Task taskIsCollision = Task.Factory.StartNew(this.CollisionCheck);

            TimerCallback timerCallback = new TimerCallback(TimerTick);
            /*Timer */
            timer = new Timer(timerCallback);

            // TODO: рандомный интервал для падений.
            timer.Change(1000, 100);

        }

        private void CollisionCheck()
        {
            while (true)
            {
                Thread.Sleep(100);
                //Console.WriteLine("check ---------");

                if (this.temp)  // TODO: вместо temp реализация проверки наложения.
                {
                    timer.Change(Timeout.Infinite, 0);
                }
            }
        }

        private void TimerTick(object state)
        {
            if (this.tasks.Count < AMOUNT_OF_ASTEROIDS_TASK)
            {
                this.tasks.Add(
                    Task.Factory.StartNew(
                        () => this.AsteroidLaunch(this.GetRandomXCoordinate())
                        )
                    );
            }


            // testing
            Console.WriteLine(tasks.Count);
            //Console.WriteLine($"Controls.Count = {this.Controls.Count}");
        }

        private int GetRandomXCoordinate()
        {
            return random.Next(this.ClientSize.Width - SIZE_ASTEROID);
        }

        private void AsteroidLaunch(int x)
        {
            Console.WriteLine("Старт метода - AsteroidLaunch");

            // Создаем астероид.
            PictureBox box = this.CreateAsteroid(x);

            // Включаем падение астероида.
            StartOfTheFall(box);
        }

        private void StartOfTheFall(PictureBox box)
        {
            while (box.Location.Y != this.ClientSize.Height)
            {
                Thread.Sleep(this.fallingSpeed);
                if (temp == false)
                {
                    box.Location = new Point(box.Location.X, box.Location.Y + 1);
                }
            }

            if (box.Location.Y == this.ClientSize.Height)
            {
                Console.WriteLine(">>>>> delete 0");
                this.tasks.RemoveAt(0);
                box.Dispose();  // удаление ненужного (упавшего) астероида.
            }
        }

        private PictureBox CreateAsteroid(int x)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.BackColor = Color.Black;
            pictureBox.Size = new Size(SIZE_ASTEROID, SIZE_ASTEROID);
            pictureBox.Location = new Point(x, 0 - SIZE_ASTEROID);
            pictureBox.Visible = true;
            
            this.Invoke(new Action<PictureBox>(this.ShowAsteroid), pictureBox);

            return pictureBox;
        }

        private void ShowAsteroid(PictureBox pictureBox)
        {
            this.Controls.Add(pictureBox);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            // TODO: уничтожение таймера.
            //this.timer.Dispose();

            temp = true;
        }
    }
}
