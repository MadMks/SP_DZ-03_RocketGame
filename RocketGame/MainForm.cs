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
        private const int AMOUNT_OF_ASTEROIDS_TASK = 2;
        private List<Task> tasks = null;


        // temp
        //private PictureBox pictureBox = null;

        public MainForm()
        {
            InitializeComponent();

            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.tasks = new List<Task>();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            TimerCallback timerCallback = new TimerCallback(TimerTick);
            Timer timer = new Timer(timerCallback);

            timer.Change(1000, 2000);
        }

        private void TimerTick(object state)
        {
            // TODO: рандомно получать X - размещение астероида. 

            if (this.tasks.Count < AMOUNT_OF_ASTEROIDS_TASK)
            {
                this.tasks.Add(
                    Task.Factory.StartNew(
                        () => this.AsteroidLaunch(10)
                        )
                    );
            }
        }

        private void AsteroidLaunch(int x)
        {
            Console.WriteLine("Старт метода - AsteroidLaunch");

            PictureBox box = this.CreateAsteroid(x);

            // TODO: включить падение астероида.
            while (box.Location.Y != this.ClientSize.Height - 20)
            {
                box.Location = new Point(box.Location.X, box.Location.Y + 1);
            }
        }

        private PictureBox CreateAsteroid(int x)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.BackColor = Color.Black;
            pictureBox.Size = new Size(20, 20);
            pictureBox.Location = new Point(x, 0);
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
