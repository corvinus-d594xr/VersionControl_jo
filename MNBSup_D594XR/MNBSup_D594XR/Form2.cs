using MNBSup_D594XR.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MNBSup_D594XR
{
    public partial class Form2 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        public Form2()
        {
            InitializeComponent();
            var er1 = from x in Rates
                      select x;
            dataGridView1.DataSource = er1.ToList();
        }
    }
}
