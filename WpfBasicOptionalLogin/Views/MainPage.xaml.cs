using System.Windows.Controls;

using WpfBasicOptionalLogin.ViewModels;

namespace WpfBasicOptionalLogin.Views
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
