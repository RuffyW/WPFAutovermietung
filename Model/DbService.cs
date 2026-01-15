using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// --------------------------------------------------------------------------------------------------------

using System.Data.SQLite; // Einbinden der SQLite Bibliothek
// Vorsicht: Pfad Visual Studio darf nicht zu lange sein,
// ggf. auf C / repos kopieren


// --------------------------------------------------------------------------------------------------------

namespace WPFAutovermietung.Model
{
    /// <summary>
    /// Klasse für die Datenbankzugriffe (SQLite)
    /// </summary>
    /// <remarks>
    /// Baut die Verbindung zur SQLite-DB auf, lädt alle
    /// Fahrzeuge und fügt ein neues Fahrzeug hinzu.
    /// Alle diese Vorgänge sind mit der DB gekoppelt.
    /// </remarks>
    public class DbService
    {
        // speichert den Connection-String
        // also: die Informationen wie SQLite die DB findet
        private string _connectionString;

        // WAS PASSIERT, WENN DIE DATEI NICHT EXISTIERT?

        /// <summary>
        /// Konstruktor prüft die Verbindung zur DB und
        /// erhält den Pfad als Übergabe.
        /// </summary>
        /// <param name="dbFilePath"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        public DbService(string dbFilePath)
        {
            // prüft ob der Pfad nicht leer ist
            if (string.IsNullOrEmpty(dbFilePath))
                throw new ArgumentException("Der Pfad der Datenbank ist leer!", nameof(dbFilePath));
            
            //prüft, ob die Datei leer ist
            if (!System.IO.File.Exists(dbFilePath))
                throw new System.IO.FileNotFoundException("Datenbankdatei existiert nicht!", nameof(dbFilePath));
            
            _connectionString = $"Data Source={dbFilePath};Version=3;";
        }

        // -----------------------------------------------------------------------------------------------------

        /// <summary>
        /// Lädt die Fahrzeugdaten aus der SQLite Datenbank
        /// </summary>
        /// <returns></returns>
        public List<Fahrzeug> LadeAlleFahrzeuge()
        {
            // hier werden die geladenen Fahrzeuge gesammelt
            var list = new List<Fahrzeug>();
            SQLiteCommand cmd;

            //Ausnahmebehandlung
            try
            {
                //Erstelle eine Verbindung zur Datenbank
                //diese soll automatisch geschlossen werden
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    //öffne die Datenbank
                    conn.Open();
                    
                    //hole alle Fahrzeuge per SQL-Befehl aus der Tabelle Fahrezeuge
                    string sql = "SELECT KFZNr, Hersteller, Typ, PreisKm, Zusatz FROM Fahrzeuge";
                    
                    //führe den Sql Befehl aus
                    using (cmd = new SQLiteCommand(sql, conn))
                    
                    
                    //gib mir ein Objekt zurück, das Zeile für Zeile iteriert
                    using (var reader = cmd.ExecuteReader())
                    {
                        //jede Zeile solle ausgelesen werden
                        while (reader.Read())
                        {
                            //holt den Wert falls Null ist, keine exception
                            //stattdessen null
                            string kfz = reader["KFZNr"]?.ToString();
                            string hersteller = reader["Hersteller"]?.ToString();
                            string typ = reader["Typ"]?.ToString();
                            string zusatz = reader["Zusatz"]?.ToString();
                            
                            //es wird ein Default wert von 0 eingesetzt, wenn der Wert nicht gefunden wird
                            decimal preis = reader["PreisKm"] != DBNull.Value ? Convert.ToDecimal(reader["PreisKm"]) : 0m;
                            
                            // Erstelle ein Fahrzeug objekt
                            Fahrzeug f;
                            
                            //entscheidung ob PKW oder LKW
                            if (!string.IsNullOrEmpty(zusatz) && zusatz.Contains("Sitze"))
                            {
                                f = new PKW();
                            }
                            else
                            {
                                f = new LKW();
                            }
                            
                            // Eigenschaften setzen
                            f.setKFZNr(kfz);
                            f.setHersteller(hersteller);
                            f.setTyp(typ);
                            f.setMietpreis_km(preis);
                            
                            //Zusatzfelder: PKW -> Anzahl Sitze, LWK -> maximale Fracht
                            if (f is PKW p && int.TryParse(zusatz?.Split(' ')[0], out int anzSitze))
                            {
                                p.setAnzSitze(anzSitze);
                            }
                            
                            if (f is LKW l && decimal.TryParse(zusatz?.Split(' ')[0], out decimal fracht))
                            {
                                l.setMaxFracht(fracht);
                            }
                            
                            //liste befüllen
                            list.Add(f);
                        }
                    }
                }
            }
            //SQLite Ausnahmebehandlung
            catch (SQLiteException ex)
            {
                Console.WriteLine($"SQLite: Fehler beim Laden der Fahrzeugdaten: {ex.Message}");
            }
            //Allgemeine Ausnahmebehandlung
            catch (Exception ex)
            {
                Console.WriteLine($"Allgemeiner Fehler beim Laden der Fahrzeugdaten: {ex.Message}");
            }

            return list;
        }

        // -----------------------------------------------------------------------------------------------------

        /// <summary>
        /// Füge in die Datenbank ein neues Fahrzeug hinzu
        /// </summary>
        /// <param name="f"></param>
        public void Insert(Fahrzeug f)
        {
            
            // Vorraussetzung: ein Fahrzeug darf keinen null-Wert haben
            if (f == null)
            {
                throw new ArgumentNullException(nameof(f));
            }
            
            try
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    //öffne die Datenbank
                    conn.Open();

                    //SQL Insert mit Parametern zum Hinzufügen eines Fahrzeugdatensatzes
                    string sql = "Insert Into Fahrzeuge (KFZNr, Hersteller, Typ, PreisKm, Zusatz) Values (@k, @h, @t, @p, @z)";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(@"k", f.getKFZNr());
                        cmd.Parameters.AddWithValue(@"h", f.getHersteller());
                        cmd.Parameters.AddWithValue(@"t", f.getTyp());
                        cmd.Parameters.AddWithValue(@"p", f.getMietpreis_km());
                        
                        
                        // Zusatzfelder
                        string zus = f is PKW pp ? $"{pp.getAnzSitze()} Sitze" : f is LKW ll ? $"{ll.getMaxFracht()} t Fracht" : "";
                        cmd.Parameters.AddWithValue(@"z", zus);
                        
                        cmd.ExecuteNonQuery(); // führe nun den Insert auf die Datenbank aus
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Allgemeiner Fehler beim Laden der Fahrzeugdaten: {ex.Message}");
            }
        }

        /// <summary>
        /// Lösche einen Datensatz aus der Datenbank
        /// </summary>
        /// <param name="kfzNr"></param>
        public void Delete(string kfzNr)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                
                string sql = $"DELETE FROM Fahrzeuge WHERE KFZNr = '{kfzNr}'";
                
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(@"k", kfzNr);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Änderungen an einem Fahrzeugdatensatz sollen in die Datenbank übernommen werden
        /// </summary>
        /// <param name="f"></param>
        
        public void Update(Fahrzeug f)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                
                //string sql = $"UPDATE Fahrzeuge SET Hersteller = '{f.getHersteller()}', Typ = '{f.getTyp()}', PreisKm = {f.getMietpreis_km()} WHERE KFZNr = '{f.getKFZNr()}'";
                string sql = $"UPDATE Fahrzeuge SET KFZNr=@k, Hersteller=@h, Typ=@t, PreisKm=@p, Zusatz=@z WHERE KFZNr=@k";
                
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(@"k", f.getKFZNr());
                    cmd.Parameters.AddWithValue(@"h", f.getHersteller());
                    cmd.Parameters.AddWithValue(@"t", f.getTyp());
                    cmd.Parameters.AddWithValue(@"p", f.getMietpreis_km());
                    cmd.Parameters.AddWithValue(@"z", f is PKW pp ? $"{pp.getAnzSitze()} Sitze" : f is LKW ll ? $"{ll.getMaxFracht()} t Fracht" : "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> AvgPreisByTyp()
        {
            var result = new List<string>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Hersteller, ROUND(AVG(PreisKm), 2) FROM Fahrzeuge GROUP BY Hersteller ORDER BY AVG(PreisKm)";

                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add($"{reader[0]}: ⌀  {reader[1]} €/km");
                    }   
                }
            }
            return result;
        }
    }
}
