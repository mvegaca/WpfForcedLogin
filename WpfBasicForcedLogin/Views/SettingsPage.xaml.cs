using System.Windows.Controls;

using WpfBasicForcedLogin.ViewModels;

namespace WpfBasicForcedLogin.Views
{
    public partial class SettingsPage : Page
    {
        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
