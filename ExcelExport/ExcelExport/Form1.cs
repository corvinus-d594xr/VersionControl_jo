using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ExcelExport
{
    public partial class Form1 : Form
    {
        RealEstateEntities context = new RealEstateEntities(); //A Form1 osztály szintjén példányosítsd az ORM objektumot! (Ha nem írtad át, akkor RealEstateEntities néven kellett létrejöjjön.)
        List<Flat> lakasok; //A Form1 osztály szintjén hozz létre egy Flat típusú elemekből álló listára mutató referenciát. (Nem kell inicializálni new-val.)
        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;

        
        public Form1()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.DataSource = lakasok;
            CreateExcel();
        }
        public void LoadData() //Hozz létre egy void visszatérésű értékű, paraméter nélküli függvényt a Form1 osztályon belül LoadData néven, és hívd meg a Form konstruktorából.
        {
            lakasok = context.Flats.ToList();
        }
        public void CreateExcel()
        {
            try
            {
                /*megnyitjuk az appot*/xlApp = new Excel.Application(); //pédányosítunk objektumokat vagy osztályokat
                /*létrehozunk egy munkafüzetet*/xlWB = xlApp.Workbooks.Add(Missing.Value); //adott applikáción belül lévő munkalapok listája
                //amikor megnyitjuk az excelt és üres dokumentumot hozunk létre-->missing.value, üres munkafüzetet akarunk átadni/létrehozni
                /*kiválasztjuk az appban levő munkalapot*/xlSheet = xlWB.ActiveSheet(); //aktív munkalapot nyisson meg a munkafüzetek közül

                CreateTable();

                xlApp.Visible = true;
                xlApp.UserControl=true;//a vezérlést átadjuk a felhasználónak
            }
            catch (Exception ex)
            {
                string hiba = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source); //nline sortörés
                //exceptionban tárolt hibaüzenetet és forrást kiírja
                MessageBox.Show(hiba, "Error"); //error-t nem kötelező odaírni, csak illik
                //FONTOS A SORREND: ELŐSZÖR ZÁROM BE A MUNKAFÜZETET, AZTÁN AZ APPOT
                xlWB.Close(false, Type.Missing, Type.Missing); //ha hiba történik, akkor bezárjuk
                xlApp.Quit(); //bezárom az appot, feladat kezelőből eltűnik, de a visul studio a memóriában tartja, ezért ki kell nullázni, h teljesen le legyen zárva
                xlWB = null; //kiürítem a munkafüzet változómat is
                xlApp = null;
            }  
        }

        private void CreateTable() //ide fogom rakni, amit bele akarok írni az excelbe
        {

        }
    }
}
