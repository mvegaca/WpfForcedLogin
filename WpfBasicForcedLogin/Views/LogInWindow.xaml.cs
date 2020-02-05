using MahApps.Metro.Controls;

using WpfBasicForcedLogin.Contracts.Views;
using WpfBasicForcedLogin.ViewModels;

namespace WpfBasicForcedLogin.Views
{
    public partial class LogInWindow : MetroWindow, ILogInWindow
    {
        public LogInWindow(LogInViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();
    }
}
