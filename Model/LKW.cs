using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFAutovermietung.Model
{
    public class LKW : Fahrzeug
    {

        protected decimal maxFracht;

        public LKW() : base()
        {
        }

        public LKW(string kfzNr, string hersteller, string typ, decimal mietpreis_km, decimal maxFracht)
            : base(kfzNr, hersteller, typ, mietpreis_km) // "Aufruf" des Basisklassen.Konstruktors
        {
            setMaxFracht(maxFracht);
        }

        public void setMaxFracht(decimal maxFracht)
        {
            this.maxFracht = maxFracht;
        }

        public decimal getMaxFracht()
        {
            return this.maxFracht;
        }

        public override void display()
        {
            base.display();
            Console.Write(", " + this.maxFracht.ToString("N2") + " to Fracht");
        }

    }
}
