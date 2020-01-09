using System.Windows.Controls;
using MahApps.Metro.Controls;

using WpfLightForcedLogin.Contracts.Views;

namespace WpfLightForcedLogin.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow
    {
        public ShellWindow()
        {
            InitializeComponent();
        }

        public Frame GetNavigationFrame()
            => shellFrame;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();
    }
}
