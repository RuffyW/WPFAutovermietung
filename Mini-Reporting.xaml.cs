using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using WPFAutovermietung.Model;

namespace WPFAutovermietung
{
    public partial class Mini_Reporting : Window
    {
        public Mini_Reporting()
        {
            InitializeComponent();
        }

        public Mini_Reporting(DbService db)
        {
            InitializeComponent();


            List<string> daten = db.AvgPreisByTyp();
       
            //List-Box füllen
            foreach (var eintrag in daten)
            {
                report.Items.Add(eintrag);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}