using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFAutovermietung.Model
{
    public abstract class Fahrzeug
    {

        protected string kfzNr = String.Empty;
        protected string hersteller = String.Empty;
        protected string typ = String.Empty;
        protected decimal mietpreis_km;

        public Fahrzeug()
        {

        }

        public Fahrzeug(string kfzNr, string hersteller, string typ, decimal mietpreis_km)
        {
            setKFZNr(kfzNr);
            setHersteller(hersteller);
            setTyp(typ);
            setMietpreis_km(mietpreis_km);
        }

        public void setKFZNr(string Nr)
        {
            this.kfzNr = Nr;
        }

        public string getKFZNr()
        {
            return this.kfzNr;
        }

        public void setHersteller(string Name)
        {
            this.hersteller = Name;
        }

        public string getHersteller()
        {
            return this.hersteller;
        }

        public void setTyp(string Typ)
        {
            this.typ = Typ;
        }

        public string getTyp()
        {
            return this.typ;
        }

        public void setMietpreis_km(decimal Preis)
        {
            this.mietpreis_km = Preis;
        }

        public decimal getMietpreis_km()
        {
            return this.mietpreis_km;
        }

        public virtual void display()
        {
            Console.WriteLine();
            Console.Write(this.GetType().Name + " " + this.kfzNr + ", " + this.hersteller + this.typ + ", " + this.mietpreis_km + " EUR/km");
        }

    }
}