﻿using System.Windows.Controls;

using WpfBasicOptionalLogin.ViewModels;

namespace WpfBasicOptionalLogin.Views
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
