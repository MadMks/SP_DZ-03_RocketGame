using RocketGame.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private const int FALL_INTERVAL = 100;

        private const int SIZE_ROCKET_X = 20;
        private const int SIZE_ROCKET_Y = 40;

        private const int SIZE_ASTEROID = 20;
        private const int AMOUNT_OF_ASTEROIDS_TASK = 3;

        private const int DISTANCE = 35;

        private List<Task> tasks = null;

        private Random random = null;
        private int fallingSpeed;

        private Timer timer = null;

        private PictureBox pictureBoxRocket = null;


        private bool isGameContinues = true;

        private Button buttonStart = null;


        
        Rectangle rectRocket = new Rectangle();


        public MainForm()
        {
            InitializeComponent();

            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.random = new Random();
            this.tasks = new List<Task>();
            this.fallingSpeed = 2;

            this.buttonStart = new Button();
            this.buttonStart.Visible = true;
            this.buttonStart.Text = "Старт";
            this.buttonStart.Location = this.GetCenterPointOfForm();
            this.buttonStart.Click += buttonStart_Click;
            this.Controls.Add(this.buttonStart);

            this.KeyPreview = true;

            this.KeyDown += MainForm_KeyDown;

        }
        
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            TimerCallback timerCallback = new TimerCallback(MoveLeft);
            Timer timer = new Timer(timerCallback);

            if (e.KeyData == Keys.Left)
            {
                timer.Dispose();

                timerCallback = MoveLeft;
                timer = new Timer(timerCallback);
                timer.Change(0, 0);
            }
            else if (e.KeyData == Keys.Right)
            {
                timer.Dispose();

                timerCallback = MoveRight;
                timer = new Timer(timerCallback);
                timer.Change(0, 0);
            }
        }

        private void MoveRight(object state)
        {
            int i = 0;
            while (i < DISTANCE)
            {
                if (pictureBoxRocket.Location.X + 1 > this.ClientSize.Width - SIZE_ROCKET_X)
                {
                    return;
                }

                //this.pictureBoxRocket.Location
                //    = new Point(
                //        this.pictureBoxRocket.Location.X + 1,
                //        this.pictureBoxRocket.Location.Y
                //        );
                this.Invoke(
                    new Action<PictureBox>(this.StepRight),
                    this.pictureBoxRocket
                    );
                i++;
            }
        }

        private void StepRight(PictureBox rocket)
        {
            rocket.Location
                = new Point(
                    rocket.Location.X + 1,
                    rocket.Location.Y
                    );
        }

        private void MoveLeft(object state)
        {
            int i = 0;
            while (i < DISTANCE)
            {
                if (pictureBoxRocket.Location.X - 1 < 0)
                {
                    return;
                }

                //this.pictureBoxRocket.Location
                //= new Point(
                //    this.pictureBoxRocket.Location.X - 1,
                //    this.pictureBoxRocket.Location.Y
                //    );
                this.Invoke(
                    new Action<PictureBox>(this.StepLeft),
                    this.pictureBoxRocket
                    );
                i++;
            }
        }

        private void StepLeft(PictureBox rocket)
        {
            rocket.Location
            = new Point(
                rocket.Location.X - 1,
                rocket.Location.Y
                );
        }

        private Point GetCenterPointOfForm()
        {
            Point pointBtnStart = new Point(
                (this.ClientSize.Width / 2) - (this.buttonStart.Width / 2),
                (this.ClientSize.Height / 2) - (this.buttonStart.Height / 2)
                );

            return pointBtnStart;
        }

        private void AddRocket()
        {
            pictureBoxRocket = new PictureBox();
            //pictureBoxRocket.BackColor = Color.Green;
            pictureBoxRocket.Image = Resources.Rocket;
            pictureBoxRocket.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxRocket.Size = new Size(SIZE_ROCKET_X, SIZE_ROCKET_Y);
            pictureBoxRocket.Location = this.StartPointRocket();
            pictureBoxRocket.Visible = true;

            this.Controls.Add(pictureBoxRocket);

            pictureBoxRocket.LocationChanged += PictureBoxRocket_LocationChanged;
            this.pictureBoxRocket.Focus();

           
            rectRocket = this.pictureBoxRocket.DisplayRectangle;
            rectRocket.Location = this.pictureBoxRocket.Location;
        }


        private void PictureBoxRocket_LocationChanged(object sender, EventArgs e)
        {
            rectRocket = this.pictureBoxRocket.DisplayRectangle;
            rectRocket.Location = this.pictureBoxRocket.Location;
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

            this.GameDesignSettings();

            // Добавление ракеты.
            this.AddRocket();


            TimerCallback timerCallback = new TimerCallback(TimerTick);

            timer = new Timer(timerCallback);

            // TODO: рандомный интервал для падений.
            timer.Change(1000, FALL_INTERVAL);

        }

        private void GameDesignSettings()
        {
            this.buttonStart.Visible = false;
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


        }

        private int GetRandomXCoordinate()
        {
            return random.Next(this.ClientSize.Width - SIZE_ASTEROID);
        }

        private void AsteroidLaunch(int x)
        {
            // Создаем астероид.
            PictureBox box = this.CreateAsteroid(x);

            // Включаем падение астероида.
            StartOfTheFall(box);
        }

        private void StartOfTheFall(PictureBox box)
        {
            while (box.Location.Y != this.ClientSize.Height)
            {
                
                if (this.isGameContinues)
                {
                    Thread.Sleep(this.fallingSpeed);

                    this.Invoke(
                        new Action<PictureBox>(this.MoveDownAsteroid), 
                        box
                        );

                    if (this.pictureBoxRocket != null)
                    {
                        Rectangle rectAsteroid = box.DisplayRectangle;
                        rectAsteroid.Location = box.Location;
                        if (rectRocket.IntersectsWith(rectAsteroid))
                        {
                            timer.Dispose();
                            this.isGameContinues = false;
                            MessageBox.Show("Game over",
                                "Результат");


                            this.RocketFall();

                            this.Invoke(
                                new Action(this.MenuDesignSettings)
                                );
                        }
                    }
                }
            }
            Console.WriteLine("Tasks[0]: " + tasks[0].Id);
            if (box.Location.Y == this.ClientSize.Height)
            {
                this.tasks.RemoveAt(0); // Удаление из списка активных тасков.
                //box.Dispose();  // удаление ненужного (упавшего) астероида.
                this.Invoke(
                    new Action<PictureBox>(this.AsteroidDestruction),
                    box
                    );
            }
            Process process = Process.GetCurrentProcess();

            Console.WriteLine("\nCount: " + process.Threads.Count + "\n");
        }

        private void AsteroidDestruction(PictureBox asteroid)
        {
            asteroid?.Dispose();
        }

        private void MoveDownAsteroid(PictureBox pBox)
        {
            pBox.Location = new Point(pBox.Location.X, pBox.Location.Y + 1);
        }

        private void MenuDesignSettings()
        {
            this.buttonStart.Visible = true;
        }

        private void RocketFall()
        {
            // После ее падения 
            // можно запустить падение оставшихся астероидов
            // после чего они уничтожатся, и игру можно будет начать заново.

            while (this.pictureBoxRocket.Location.Y != this.ClientSize.Height + SIZE_ASTEROID)
            {
                //pictureBoxRocket.Location 
                //    = new Point(
                //        pictureBoxRocket.Location.X,
                //        pictureBoxRocket.Location.Y + 1);

                this.Invoke(
                        new Action<PictureBox>(this.MoveDownAsteroid),
                        pictureBoxRocket
                        );
            }
            if (pictureBoxRocket.Location.Y == this.ClientSize.Height + SIZE_ASTEROID)
            {
                //this.Controls.Remove(this.pictureBoxRocket);        // FIX: HACK: ???
                this.Invoke(
                    new Action<PictureBox>(this.RemovingRocketFromForm),
                    this.pictureBoxRocket
                    );
                //pictureBoxRocket?.Dispose();  // удаление ракеты.   // FIX: HACK: ???
                this.Invoke(
                    new Action<PictureBox>(this.RocketDestruction),
                    this.pictureBoxRocket
                    );
            }

            this.isGameContinues = true;
        }

        private void RocketDestruction(PictureBox obj)
        {
            pictureBoxRocket?.Dispose();
        }

        private void RemovingRocketFromForm(PictureBox rocket)
        {
            this.Controls.Remove(rocket);
        }

        private PictureBox CreateAsteroid(int x)
        {
            PictureBox pictureBox = new PictureBox();
            //pictureBox.BackColor = Color.Black;
            pictureBox.Image = Resources.Asteroid;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
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


    }
}
