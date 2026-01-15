using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFAutovermietung.Model
{
    class Autovermietung
    {

        protected string bezeichnung = String.Empty;
        protected List<Fahrzeug> fahrzeuge = new List<Fahrzeug>();
        protected static int anzAutovermietungen;

        ///
        public Autovermietung()
        {
            anzAutovermietungen++;
        }

        public void setBezeichnung(string bezeichnung)
        {
            this.bezeichnung = bezeichnung;
        }

        public string getBezeichnung()
        {
            return this.bezeichnung;
        }

        public void addFahrzeug(Fahrzeug fahrz)
        {
            fahrzeuge.Add(fahrz);
        }

        public int getAnzFahrzeuge()
        {
            return fahrzeuge.Count;
        }

        public Fahrzeug getFahrzeug(int index)
        {
            return fahrzeuge[index];
        }

        public static int getAnzAutovermietungen()
        {
            return anzAutovermietungen;
        }

        public void display()
        {
            Console.WriteLine("Autovermietung \"" + this.bezeichnung + "\" (" + anzAutovermietungen + ". Autovermietung)\n");
            const string linie = "----------";
            Console.Write(linie + getAnzFahrzeuge() + " Fahrzeuge " + linie);
            foreach (Fahrzeug fahrzeug in this.fahrzeuge)
            {
                // hier müssen wir später noch etwas aufrufen
                fahrzeug.display();
            }
            Console.WriteLine();
        }


        // muss ergänzt werden zur ursprünglichen Version
        public List<Fahrzeug> GetFahrzeuge()
        {
            return fahrzeuge;
        }

        // neu hinzugefügt um KFZ-Nr suchen zu können
        // Button bearbeiten
        public Fahrzeug Suche(string kfz)
        {
            foreach (var f in fahrzeuge)
            {
                if (f.getKFZNr() == kfz)
                    return f;
            }
            return null;
        }

        // neue hinzugefügt um Datensatz löschen zu können
        // Button löschen
        public void LoescheNachKennzeichen(string kfz)
        {
            var f = Suche(kfz);
            if (f != null)
                fahrzeuge.Remove(f);
        }

    }
}
