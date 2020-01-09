using MahApps.Metro.Controls;
using WpfLightForcedLogin.Contracts.Views;

namespace WpfLightForcedLogin.Views
{
    public partial class LogInWindow : MetroWindow, ILogInWindow
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        public void CloseWindow()
            => Close();

        public void ShowWindow()
            => Show();
    }
}
