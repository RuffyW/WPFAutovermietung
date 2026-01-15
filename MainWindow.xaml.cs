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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFAutovermietung.Finanzrechner.View;
using WPFAutovermietung.Model;


namespace WPFAutovermietung
{
    /// <summary>
    /// Klasse für die Interaktionslogik
    /// </summary>
    public partial class MainWindow : Window
    {

        // für die Teil-Funktionalität 'Metronom'
        private Metronom metronom;

        // EINBINDEN FÜR DEN DATENBANKZUGRIFF
        private DbService db; // hier Klasse DbService einbinden

        private Autovermietung sixt;

        /// <summary>
        /// initialisiert das Hauptfenster
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // // EINBINDEN FÜR DEN DATENBANKZUGRIFF, hier wurde der Pfad absolut gesetzt
            string dbPfad = @"C:\Users\weidinger\Desktop\Autovermietung.db";
            db = new DbService(dbPfad);
            
            // db = new DbService("autovermietung.db"); // DB-Service initialisieren
        }

        /// <summary>
        /// Lädt die Fahrzeugdaten bei Klick in die Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Der Ereignishandler wird durch Klick auf den Button 'BtnLaden' ausgeführt.
        /// </remarks>
        private void BtnLaden_Click(object sender, RoutedEventArgs e)
        {

            // über Datenbankzugriff NEU 03.12.2025
            sixt = new Autovermietung();
            foreach (var f in db.LadeAlleFahrzeuge())
                sixt.addFahrzeug(f);



            AktualisiereGrid();
            txtStatus.Text = "Datenbank geladen!";



        }

        /// <summary>
        /// Fügt Fahrzeugdaten der Grid hinzu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Über die Eingabefelder kann ein Fahrzeugdatensatz der Grid hinzugefügt werden.
        /// </remarks>
        private void BtnHinzufuegen_Click(object sender, RoutedEventArgs e)
        {
            if (sixt == null)
            {
                MessageBox.Show("Bitte zuerst Fahrzeuge laden!");
                return;
            }

            // Validierung
            if (string.IsNullOrWhiteSpace(txtKFZ.Text) ||
                string.IsNullOrWhiteSpace(txtHersteller.Text) ||
                string.IsNullOrWhiteSpace(txtModell.Text) ||
                !decimal.TryParse(txtPreis.Text, out decimal preis))
            {
                MessageBox.Show("Bitte alle Felder korrekt ausfüllen!");
                return;
            }

            // Neuen PKW erzeugen
            PKW p = new PKW();
            p.setKFZNr(txtKFZ.Text);
            p.setHersteller(txtHersteller.Text);
            p.setTyp(txtModell.Text);
            p.setMietpreis_km(preis);
            p.setAnzSitze(5);       // Beispielwert

            sixt.addFahrzeug(p);


            db.Insert(p); //NEU zum Schreiben in die Datenbank, NEU 03.12.2025


            // Refresh der Anzeige
            AktualisiereGrid();

            txtStatus.Text = $"Fahrzeug {p.getKFZNr()} hinzugefügt.";

        }

        private void AktualisiereGrid()
        {
            var anzeigen = sixt.GetFahrzeuge()
                .Select(f => new
                {
                    Fahrzeugtyp = f.GetType().Name,
                    Kennzeichen = f.getKFZNr(),
                    Hersteller = f.getHersteller(),
                    Modell = f.getTyp(),
                    Preis = $"{f.getMietpreis_km():0.00} €/km",
                    Zusatz = f is PKW p ? $"{p.getAnzSitze()} Sitze"
                           : f is LKW l ? $"{l.getMaxFracht()} t Fracht"
                           : ""
                })
                .ToList();

            dgFahrzeuge.ItemsSource = null;
            dgFahrzeuge.ItemsSource = anzeigen;

        }

        // dadurch füllen wir die Eingabefelder
        // dieser Ereignishandler tritt immer auf, wenn
        // die Auswahl eines Selectors in der DataGrid markiert wird
        private void DgFahrzeuge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgFahrzeuge.SelectedItem == null) return;

            // mit dynamic können Objekte benutz werden, obwohl deren Typ nicht bekannt ist
            // wir verwenden im DataGrid anonyme Objekte
            // deshalb dynamic um auf deren Eigenschaften zugreifen zu können
            dynamic f = dgFahrzeuge.SelectedItem;

            txtKFZ.Text = f.Kennzeichen;
            txtHersteller.Text = f.Hersteller;
            txtModell.Text = f.Modell;
            txtPreis.Text = f.Preis.Replace("€/km", "").Trim();
        }

        private void BtnBearbeiten_Click(object sender, RoutedEventArgs e)
        {
            if (dgFahrzeuge.SelectedItem == null)
            {
                MessageBox.Show("Bitte ein Fahrzeug auswählen!");
                return;
            }

            dynamic selected = dgFahrzeuge.SelectedItem;

            var fahrzeug = sixt.Suche(selected.Kennzeichen);

            fahrzeug.setHersteller(txtHersteller.Text);
            fahrzeug.setTyp(txtModell.Text);
            fahrzeug.setMietpreis_km(decimal.Parse(txtPreis.Text));
            
            db.Update(fahrzeug);

            AktualisiereGrid();
            txtStatus.Text = $"Fahrzeug {fahrzeug.getKFZNr()} geändert.";
        }

        private void BtnLoeschen_Click(object sender, RoutedEventArgs e)
        {
            if (dgFahrzeuge.SelectedItem == null)
            {
                MessageBox.Show("Bitte ein Fahrzeug auswählen!");
                return;
            }

            dynamic selected = dgFahrzeuge.SelectedItem;

            sixt.LoescheNachKennzeichen(selected.Kennzeichen);
            
            db.Delete(selected.Kennzeichen);

            AktualisiereGrid();
            txtStatus.Text = $"Fahrzeug {selected.Kennzeichen} gelöscht.";
        }

        private void BtnKosten_Click(object sender, RoutedEventArgs e)
        {
            if (dgFahrzeuge.SelectedItem == null)
            {
                MessageBox.Show("Bitte ein Fahrzeug auswählen!");
                return;
            }

            // Dialog öffnen
            Mietrechner dlg = new Mietrechner("Gefahrene Kilometer:");
            dlg.Owner = this;

            if (dlg.ShowDialog() != true)
                return; // Abgebrochen

            if (!decimal.TryParse(dlg.EingabeText, out decimal km) || km < 0)
            {
                MessageBox.Show("Ungültige Kilometerangabe!");
                return;
            }

            // Der Code wird erst beim Ausführen geprüft
            dynamic f = dgFahrzeuge.SelectedItem;

            var fahrzeug = sixt.Suche(f.Kennzeichen);

            decimal kosten = km * fahrzeug.getMietpreis_km();

            MessageBox.Show($"Mietkosten: {kosten:0.00} €", "Berechnung");
        }


        private int tickCount = 0; // Anzahl Ticks anzeigen

        // für Metronom-Funktion
        private void BtnStartTick_Click(object sender, RoutedEventArgs e)
        {
            if (metronom == null)
            {
                metronom = new Metronom(1000); // 1 Sekunde pro Tick
                metronom.Tick += Metronom_Tick;
            }

            metronom.Start();
            txtStatus.Text = "Metronom gestartet!";
        }

        // für Metronom-Funktion
        private void BtnStopTick_Click(object sender, RoutedEventArgs e)
        {
            metronom?.Stop();
            txtStatus.Text = "Metronom gestoppt.";
        }

        // für Metronom-Funktion
        private void Metronom_Tick()
        {
            // Update auf dem UI-Thread!
            Dispatcher.Invoke(() =>
            {
                tickCount++;
                txtTickCounter.Text = $"Takt: {tickCount}";
            });
        }

        private void MenuMiniReporting_Click(object sender, RoutedEventArgs e)
        {
            Mini_Reporting window = new Mini_Reporting(db);
            window.Show();
        }

        private void Finanzrechner_Click(object sender, RoutedEventArgs e)
        {
            FinanzrechnerDatenbindung window = new FinanzrechnerDatenbindung();
            window.Show();
        }

        private void Mietrechner_Click(object sender, RoutedEventArgs e)
        {
            MietrechnerDatenbindung window = new MietrechnerDatenbindung();
            window.Show();
        }

    }
}
