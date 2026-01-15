using System.Windows;
using WPFAutovermietung.Finanzrechner.ViewModel;

namespace WPFAutovermietung.Finanzrechner.View
{
    public partial class FinanzrechnerDatenbindung : Window
    {
        public FinanzrechnerDatenbindung()
        {
            InitializeComponent();
            DataContext = new FinanzrechnerViewModel();
        }
    }
}