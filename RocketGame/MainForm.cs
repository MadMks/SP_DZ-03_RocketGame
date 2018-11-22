using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RocketGame
{
    public partial class MainForm : Form
    {
        private Game game = null;
        public Game GameProcess
        {
            get { return game; }
            set { game = value; }
        }

        private Button buttonStart = null;

        public MainForm()
        {
            InitializeComponent();

            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            game = new Game(this);


            this.CreateButtonStart();
            this.ShowButtonStart();
        }

        private void ShowButtonStart()
        {
            this.buttonStart.Visible = true;
        }

        private void CreateButtonStart()
        {
            this.buttonStart = new Button();
            this.buttonStart.Visible = false;
            this.buttonStart.Text = "Старт";
            this.buttonStart.Location = this.GetCenterPointOfForm();
            this.buttonStart.Click += buttonStart_Click;
            this.Controls.Add(this.buttonStart);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.HideButtonStart();
            game.Start();
        }

        private void HideButtonStart()
        {
            this.buttonStart.Visible = false;
        }

        private Point GetCenterPointOfForm()
        {
            Point pointBtnStart = new Point(
                (this.ClientSize.Width / 2) - (this.buttonStart.Width / 2),
                (this.ClientSize.Height / 2) - (this.buttonStart.Height / 2)
                );

            return pointBtnStart;
        }
    }
}
