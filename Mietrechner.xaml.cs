using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFAutovermietung
{
    /// <summary>
    /// Interaktionslogik für Mietrechner.xaml
    /// </summary>
    public partial class Mietrechner : Window
    {

        /// <summary>
        /// hier handelt es sich um eine
        /// Read-Only-Property
        /// </summary>
        /// <remarks>
        /// Alternative Schreibweise:
        /// public string EingabeText
        /// {
        ///     get
        ///     {
        ///         return txtAntwort.Text;
        ///     }
        /// }
        /// 
        /// Was macht die Property?
        /// Sie liefert den Text zurück, den der Benutzer im Eingabefeld txtAntwort eingibt.
        /// EingabeText ist nur lesbar (kein Setter)
        /// Sie greift auf das Textfeld im Dialogfenster zu
        /// Sie wird vom MainWindow verwendet, um den eingegebenen Wert abzurufen
        /// 
        /// Kurz gesagt: EingabeText ist eine Eigenschaft,
        /// die einfach nur den Text aus dem Eingabefeld txtAntwort weitergibt.
        /// </remarks>
        public string EingabeText => txtAntwort.Text;

        /// <summary>
        /// Erzeugt das Fenster Mietrechner
        /// </summary>
        /// <param name="frage"></param>
        /// <remarks>
        /// Der Konstruktor wird aufgerufen, wenn im Hauptfenster
        /// z. B. eingegeben wird: Mietrechner dlg = new Mietrechner("Gib die gefahrenen Kilometer ein:");
        /// Es entsteht ein neues Dialogfenster, ähnlich wie ein Objekt einer Klasse.
        /// </remarks>
        public Mietrechner(string frage)
        {
            InitializeComponent();
            txtFrage.Text = frage;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

    }
}
