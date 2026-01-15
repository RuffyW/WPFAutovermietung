using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFAutovermietung.Model
{
    public class PKW : Fahrzeug
    {

        protected int anzSitze;

        public void setAnzSitze(int AnzSitze)
        {
            this.anzSitze = AnzSitze;
        }

        public int getAnzSitze()
        {
            return anzSitze;
        }

        public override void display()
        {
            base.display();
            Console.Write(", " + this.anzSitze + " Sitze");
        }

    }
}
