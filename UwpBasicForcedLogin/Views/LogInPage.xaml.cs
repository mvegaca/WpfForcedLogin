using System;

using UwpBasicForcedLogin.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UwpBasicForcedLogin.Views
{
    public sealed partial class LogInPage : Page
    {
        public LogInViewModel ViewModel { get; } = new LogInViewModel();

        public LogInPage()
        {
            InitializeComponent();
        }
    }
}
