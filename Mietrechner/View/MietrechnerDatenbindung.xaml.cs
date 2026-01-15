using System.Windows;
using WPFAutovermietung.Finanzrechner.ViewModel;

namespace WPFAutovermietung.Finanzrechner.View
{
    public partial class MietrechnerDatenbindung : Window
    {
        public MietrechnerDatenbindung()
        {
            InitializeComponent();
            DataContext = new MietrechnerViewModel();
        }
    }
}