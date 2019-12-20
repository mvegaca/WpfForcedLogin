using System;

using UwpBasicForcedLogin.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UwpBasicForcedLogin.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
