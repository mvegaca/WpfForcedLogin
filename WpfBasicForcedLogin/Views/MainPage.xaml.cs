using System.Windows.Controls;

using WpfBasicForcedLogin.ViewModels;

namespace WpfBasicForcedLogin.Views
{
    public partial class MainPage : Page
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
