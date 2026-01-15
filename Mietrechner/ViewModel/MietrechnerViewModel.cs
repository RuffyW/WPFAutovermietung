using System.ComponentModel;

namespace WPFAutovermietung.Finanzrechner.ViewModel
{
    public class MietrechnerViewModel : INotifyPropertyChanged
    {
        //Berechungsregeln
        // GrundPreis = Mietdauer * Tagespreis
        // Vollkasko 15€ / tag
        // Rabatt: Prozentualer Abzug vom Gesamtpreis
        // Ergebnis: Gesamtmietpreis

        private double tagespreis;
        private double rabatt;
        private bool vollkasko;
        private int mietdauer;
        
        public double Tagespreis
        {
            get => tagespreis;
            set
            {
                tagespreis = value;
                OnPropertyChanged(nameof(Tagespreis));
                OnPropertyChanged(nameof(Ergebnis));
            }
        }

        public double Rabatt
        {
            get => rabatt;
            set
            {
                if (rabatt != value)
                {
                    rabatt = value;
                    OnPropertyChanged(nameof(Rabatt));
                    OnPropertyChanged(nameof(Ergebnis));
                }
            }
        }

        public bool Vollkasko
        {
            get => vollkasko;
            set
            {
                if (vollkasko != value)
                {
                    vollkasko = value;
                    OnPropertyChanged(nameof(Vollkasko));
                    OnPropertyChanged(nameof(Ergebnis));
                }
            }
        }
        
        public int Mietdauer
        {
            get => mietdauer;
            set
            {
                mietdauer = value;
                OnPropertyChanged(nameof(Mietdauer));
                OnPropertyChanged(nameof(Ergebnis));
            }
        }

        /// <summary>
        /// Ergebnis des Mietpreises
        /// </summary>

        public string Ergebnis
        {
            get
            {
                double gesamtPreis = tagespreis * mietdauer;

                // Wenn vollkasko ausgewählt gesamtpreis + 15€ pro Miettag
                if (vollkasko)
                {
                    int vollkaskoTage = mietdauer * 15;
                    gesamtPreis += vollkaskoTage;
                }
                // Rabatt anwenden, wenn vorhanden
                if (rabatt > 0)
                {
                    gesamtPreis *= (1 - rabatt / 100);
                }
                return $"Der Mietpreis beträgt {gesamtPreis:F2} €";
            }
    }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    }


