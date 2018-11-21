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

        private const int DISTANCE = 50;

        private List<Task> tasks = null;

        private Random random = null;
        private int fallingSpeed;

        private Timer timer = null;

        private PictureBox pictureBoxRocket = null;


        private bool isGameContinues = true;

        private Button buttonStart = null;

        //private Task taskRocketMovement = null;

        // tes
        Rectangle rectRocket = new Rectangle();


        // temp
        //private PictureBox pictureBox = null;
        //private bool temp = false;

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
            Console.WriteLine();
            Console.WriteLine(e.KeyData);
            Console.WriteLine();

            TimerCallback timerCallback = new TimerCallback(MoveLeft);
            Timer timer = new Timer(timerCallback);

            if (e.KeyData == Keys.Left)
            {
                timer.Dispose();
                //this.MoveLeft();
                timerCallback = MoveLeft;
                timer = new Timer(timerCallback);
                timer.Change(0, 0);
            }
            else if (e.KeyData == Keys.Right)
            {
                timer.Dispose();
                //this.MoveRight();
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
                this.pictureBoxRocket.Location
                    = new Point(
                        this.pictureBoxRocket.Location.X + 1,
                        this.pictureBoxRocket.Location.Y
                        );
                i++;
            }
        }

        private void MoveLeft(object state)
        {
            int i = 0;
            while (i < DISTANCE)
            {
                this.pictureBoxRocket.Location
                = new Point(
                    this.pictureBoxRocket.Location.X - 1,
                    this.pictureBoxRocket.Location.Y
                    );
                i++;
            }
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
            /*PictureBox */pictureBoxRocket = new PictureBox();
            pictureBoxRocket.BackColor = Color.Green;
            pictureBoxRocket.Size = new Size(SIZE_ROCKET_X, SIZE_ROCKET_Y);
            pictureBoxRocket.Location = this.StartPointRocket();
            pictureBoxRocket.Visible = true;

            this.Controls.Add(pictureBoxRocket);

            // test
            pictureBoxRocket.LocationChanged += PictureBoxRocket_LocationChanged;
            this.pictureBoxRocket.Focus();

            // Событие на движение ракеты.
            //this.taskRocketMovement = Task.Factory.StartNew(this.CheckRocketMovement);

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
            /*Timer */
            timer = new Timer(timerCallback);

            // TODO: рандомный интервал для падений.
            timer.Change(1000, 100);

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
                if (this.isGameContinues)
                {
                    box.Location = new Point(box.Location.X, box.Location.Y + 1);

                    if (this.pictureBoxRocket != null)
                    {
                        Rectangle rectAsteroid = box.DisplayRectangle;
                        rectAsteroid.Location = box.Location;
                        if (rectRocket.IntersectsWith(rectAsteroid))
                        {
                            Console.WriteLine("========================---------");
                            //timer.Change(Timeout.Infinite, 0);
                            timer.Dispose();
                            this.isGameContinues = false;
                            MessageBox.Show("Game over");
                            this.RocketFall();

                            this.MenuDesignSettings();
                        }
                    }
                }
            }

            if (box.Location.Y == this.ClientSize.Height)
            {
                Console.WriteLine(">>>>> delete 0");
                this.tasks.RemoveAt(0);
                box.Dispose();  // удаление ненужного (упавшего) астероида.
            }
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
                pictureBoxRocket.Location 
                    = new Point(
                        pictureBoxRocket.Location.X,
                        pictureBoxRocket.Location.Y + 1);
            }
            if (pictureBoxRocket.Location.Y == this.ClientSize.Height + SIZE_ASTEROID)
            {
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> delete Rocket");
                //this.tasks.RemoveAt(0);
                this.Controls.Remove(this.pictureBoxRocket);        // FIX: HACK: ???
                pictureBoxRocket?.Dispose();  // удаление ракеты.   // FIX: HACK: ???
            }

            this.isGameContinues = true;
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


    }
}
