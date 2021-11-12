using _7gyak_D594XR.Abstractions;
using _7gyak_D594XR.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _7gyak_D594XR
{
    public partial class Form1 : Form
    {
        List<Toy> _toys = new List<Toy>();

        private IToyFactory factory;

        Toy _nextToy;

        public IToyFactory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        public Form1()
        {
            InitializeComponent();
            Factory = new CarFactory();
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var ball = Factory.CreateNew();
            _toys.Add(ball);
            mainPanel.Controls.Add(ball);
            ball.Left = ball.Width * (-1);

        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var maxPosition = 0;
            foreach (var item in _toys)
            {
                item.MoveToy();
                if (item.Left > maxPosition)
                {
                    maxPosition = item.Left;
                }
            }
            if (maxPosition > 1000)
            {
                Toy ba = _toys.First();
                _toys.Remove(ba);
                mainPanel.Controls.Remove(ba);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Factory = new CarFactory();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Factory = new BallFactory();
        }

        void DisplayNext()
        {

        }
    }
}
