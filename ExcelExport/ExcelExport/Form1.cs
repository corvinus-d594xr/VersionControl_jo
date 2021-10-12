using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelExport
{
    public partial class Form1 : Form
    {
        RealEstateEntities context = new RealEstateEntities(); //A Form1 osztály szintjén példányosítsd az ORM objektumot! (Ha nem írtad át, akkor RealEstateEntities néven kellett létrejöjjön.)
        List<Flat> lakasok; //A Form1 osztály szintjén hozz létre egy Flat típusú elemekből álló listára mutató referenciát. (Nem kell inicializálni new-val.)
        public Form1()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.DataSource = lakasok;
        }
        public void LoadData() //Hozz létre egy void visszatérésű értékű, paraméter nélküli függvényt a Form1 osztályon belül LoadData néven, és hívd meg a Form konstruktorából.
        {
            lakasok = context.Flats.ToList();
        }
    }
}
