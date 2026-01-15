using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFAutovermietung.Finanzrechner.ViewModel
{
    /// <summary>
    /// Klasse FinanzrechnerViewModel für bidirektionale Steuerung
    /// </summary>
    public class FinanzrechnerViewModel : INotifyPropertyChanged
    {
        private double kredit = 25000;
        private double zins = 5.5;
        private int laufzeit = 60;

        //Was soll gemacht werden?
        // Es soll z.B. der Slider Kredit geändert werden, -> dann ruft WPF set auf
        // dann: wird der Wert gespeichert und soll die betroffenden Stellen
        // weitergegeben werden

        // Das heißt, das ViewModel sagt:
        // Kredit hat sich geändert, Rate ändert sich somit auch

        
        /// <summary>
        /// Parameter Kredit
        /// </summary>
        /// Property Kredit
        public double Kredit
        {
            get => kredit;
            set
            {
                kredit = value; // setzen des Wertes für Kreditsumme
                OnPropertyChanged(nameof(Kredit));
                OnPropertyChanged(nameof(Rate));
            }
        }

        
        /// <summary>
        /// Parameter Zins
        /// </summary>
        /// Property Zins
        public double Zins
        {
            get => zins;
            set
            {
                zins = value; // setzen des Wertes für Kreditsumme
                OnPropertyChanged(nameof(Zins));
                OnPropertyChanged(nameof(Rate));
            }
        }
        
        // Property Laufzeit
        public int Laufzeit
        {
            get => laufzeit;
            set
            {
                laufzeit = value; // setzen des Wertes für Kreditsumme
                OnPropertyChanged(nameof(Laufzeit));
                OnPropertyChanged(nameof(Rate));
            }
        }
        
        /// <summary>
        /// Parameter Rate (Ergebnis)
        /// </summary>
        /// Property Rate
        public string Rate {
            get
            {
                double monatlicherZins = zins / 1200;
                double rate = Kredit * monatlicherZins * Math.Pow(1 + monatlicherZins, Laufzeit) / (Math.Pow(1 + monatlicherZins, Laufzeit) - 1);
                
                return $"Die monatliche Rate beträgt: {rate:F2} €";
            }
    }









public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}