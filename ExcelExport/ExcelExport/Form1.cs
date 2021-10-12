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
        private int million = (int)Math.Pow(10,6); //10millió
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
                xlApp = new Excel.Application(); /*megnyitjuk az appot*/ //pédányosítunk objektumokat vagy osztályokat
                /*létrehozunk egy munkafüzetet*/
                xlWB = xlApp.Workbooks.Add(Missing.Value); //adott applikáción belül lévő munkalapok listája
                //amikor megnyitjuk az excelt és üres dokumentumot hozunk létre-->missing.value, üres munkafüzetet akarunk átadni/létrehozni
                xlSheet = xlWB.ActiveSheet; /*kiválasztjuk az appban levő munkalapot*/ //aktív munkalapot nyisson meg a munkafüzetek közül

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
            string[] headers = new string[] //oszlopok elnevezései
            {
                "Kód",
                "Eladó",
                "Oldal",
                "Kerület",
                "Lift",
                "Szobák száma",
                "Alapterület (m2)",
                "Ár (mFt)",
                "Négyzetméter ár (Ft/m2)"
            };

            for (int i = 0; i < headers.Length; i++)
                xlSheet.Cells[1, i+1/*hozzáadunk 1-et h ez is 1től legyen indexelve*/] = headers[i];
           
            object[,] values=new object[lakasok.Count, headers.Length]; //összes elem

            int counter=0;
            int floorColumn = 6;

            foreach (var lakas in lakasok)
            {
                values[counter, 0]=lakas.Code;
                values[counter, 1] = lakas.Vendor;
                values[counter, 2] = lakas.Side;
                values[counter, 3] = lakas.District;
                if (lakas.Elevator == true)
                {
                    values[counter, 4] = "Van";
                }
                else 
                {
                    values[counter, 4] = "Nincs";
                }
                
                values[counter, 5] = lakas.NumberOfRooms;
                values[counter, 6] = lakas.FloorArea;
                values[counter, 7] = lakas.Price;
                values[counter, 8] = string.Format("={0}/{1}*{2}",
                    "H"+(counter+2).ToString(),
                    GetCell(counter + 2,floorColumn+1),
                    million.ToString());
                counter++;
            }
            var range = xlSheet.get_Range(
                GetCell(2,1),
                GetCell(1+values.GetLength(0), values.GetLength(1))/*első érték, olyan mintha lakások count lenne,csak könnyebben változik*/
                ); //két paraméteres fv 
            range.Value2 = values;
        }
        //Hozd létre az alábbi segéd függvényt az Excel koordináták meghatározásához.
        //szöveggé alakítja az excel koordinátákat
        //x-et és y-t adok be --> két egész szám értéket és visszakapok egy string-et
        //a GetCell lehetővé teszi koordináták alapján, hogy milyen az excel által használt koordináta
        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }

    }
}
