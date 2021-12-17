using Mikro_D594XR.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mikro_D594XR
{
    public partial class Form1 : Form
    {
        Random rng = new Random();
        List<Person> Population = null;
        List<BirthProbability> BirthProbabilities = null;//azért null mert úgyis feltöltjük csv-vel szóval itt nem kell a new-val kombinálni
        List<DeathProbability> DeathProbabilities = null;
        public Form1()
        {
            InitializeComponent();

            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");//
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");

            
        }

        private void StartSimulation(int endYear, string csvPath)
        {
            Population = GetPopulation(csvPath);
            // Végigmegyünk a vizsgált éveken
            for (int year = 2005;  year<= endYear; year++)//endyear addig megyek amig a felhasználó megadta
            {
                // Végigmegyünk az összes személyen
                for (int i = 0; i < Population.Count; i++)
                {
                    // Ide jön a szimulációs lépés
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                textBox1.Text+=(
                    string.Format("Szimulációs év:{0}\n\t Fiúk:{1}\n\t Lányok:{2}\n\n", year, nbrOfMales, nbrOfFemales));//kerdes
            }
        }

        private void SimStep(int year, Person person)
        {
            //Ha halott akkor kihagyjuk, ugrunk a ciklus következő lépésére
            if (!person.IsAlive) return;

            // Letároljuk az életkort, hogy ne kelljen mindenhol újraszámolni
            byte age = (byte)(year - person.BirthYear);

            // Halál kezelése
            // Halálozási valószínűség kikeresése
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.P).FirstOrDefault();
            // Meghal a személy?
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            //Születés kezelése - csak az élő nők szülnek
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                //Szülési valószínűség kikeresése
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.P).FirstOrDefault();
                //Születik gyermek?
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }

        public List<Person> GetPopulation(string csvpath)//egy emberekből álló listát kell visszaadnunk, string csvpath-->itt kapom meg, hogy milyen típusú fájlt szeretnék beolvasni
        {
            List<Person> population = new List<Person>();
            //fájl felolvasás
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                //sr.ReadLine(); ha van header, azt csak beolvasom és utána nem foglalkozok vele
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birthProbabilities = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthProbabilities.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P = double.Parse(line[2].Replace(".", ","))
                    }) ;
                }
            }

            return birthProbabilities;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deathProbabilities = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbabilities.Add(new DeathProbability()
                    {
                        
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        P = double.Parse(line[2].Replace(".",","))
                    });
                }
            }

            return deathProbabilities;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartSimulation((int)numericUpDown1.Value, textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.FileName = textBox1.Text;
            if (ofd.ShowDialog()!=DialogResult.OK)
            {
                return;
            }
            textBox1.Text = ofd.FileName;
        }
    }
}
