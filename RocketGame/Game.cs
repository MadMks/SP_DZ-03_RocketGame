using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace RocketGame
{
    public class Game
    {
        private const int AMOUNT_OF_ASTEROIDS_TASK = 3;

        private MainForm form = null;
        private Rocket rocket = null;
        public Rocket Rocket
        {
            get { return this.rocket; }
        }

        private Random random = null;
        private Timer timer = null;

        //private List<Task> listTasksAsteroids = null;
        public List<Task> ListTasksAsteroids { get; set; }
        public bool IsContinues { get; set; }

        public Game(MainForm form)
        {
            this.form = form;

            this.IsContinues = true;

            this.random = new Random();
            this.ListTasksAsteroids = new List<Task>();

            rocket = new Rocket();
            rocket.Location = this.StartPointRocket(form);
        }

        private Point StartPointRocket(Form form)
        {
            Point point = new Point();
            point.X = (form.ClientSize.Width / 2) - (rocket.Width / 2);
            point.Y = form.ClientSize.Height - rocket.Height;

            return point;
        }

        internal void Start()
        {
            ShowRocket();

            AsteroidsFallLaunch();

            // test
            Console.WriteLine(form.Controls.Count);
            Console.WriteLine(rocket.Location.ToString());
        }

        private void AsteroidsFallLaunch()
        {
            TimerCallback timerCallback = new TimerCallback(TimerTick);

            timer = new Timer(timerCallback);

            // TODO: рандомный интервал для падений.
            timer.Change(1000, 100);
        }

        internal void StopFallingAsteroids()
        {
            timer.Dispose();
        }

        private void TimerTick(object state)
        {
            if (this.ListTasksAsteroids.Count < AMOUNT_OF_ASTEROIDS_TASK)
            {
                this.ListTasksAsteroids.Add(
                    Task.Factory.StartNew(
                        () => this.AsteroidLaunch(this.GetRandomXCoordinate())
                        )
                    );
            }
        }

        private int GetRandomXCoordinate()
        {
            return random.Next(form.Width - Asteroid.MaxAsteroidSize);
        }

        private void AsteroidLaunch(int x)
        {
            // Создаем астероид.
            Asteroid asteroid = new Asteroid(form, x);
            // Включаем падение астероида.
            //form.Invoke(new Action(asteroid.StartOfTheFall));
            asteroid.StartOfTheFall();
        }

        private void ShowRocket()
        {
            this.form.Controls.Add(rocket);
        }
    }
}
