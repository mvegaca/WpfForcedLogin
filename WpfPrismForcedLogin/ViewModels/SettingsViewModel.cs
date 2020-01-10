using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using WpfBasicForcedLogin.Core.Contracts.Services;
using WpfPrismForcedLogin.Constants;
using WpfPrismForcedLogin.Contracts.Services;
using WpfPrismForcedLogin.Models;
using WpfPrismForcedLogin.Views;

namespace WpfPrismForcedLogin.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly AppConfig _config;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly IUserDataService _userDataService;
        private readonly IIdentityService _identityService;
        private readonly LogInWindow _logInWindow;
        private AppTheme _theme;
        private string _versionDescription;
        private ICommand _setThemeCommand;
        private ICommand _privacyStatementCommand;
        private ICommand _logoutCommand;
        private UserViewModel _user;

        public AppTheme Theme
        {
            get { return _theme; }
            set { SetProperty(ref _theme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { SetProperty(ref _versionDescription, value); }
        }

        public UserViewModel User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public ICommand SetThemeCommand => _setThemeCommand ?? (_setThemeCommand = new DelegateCommand<string>(OnSetTheme));

        public ICommand PrivacyStatementCommand => _privacyStatementCommand ?? (_privacyStatementCommand = new DelegateCommand(OnPrivacyStatement));

        public ICommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new DelegateCommand(OnLogout));

        public SettingsViewModel(IRegionManager regionManager, AppConfig config, IThemeSelectorService themeSelectorService, IUserDataService userDataService, IIdentityService identityService, LogInWindow logInWindow)
        {
            _regionManager = regionManager;
            _config = config;
            _themeSelectorService = themeSelectorService;
            _userDataService = userDataService;
            _identityService = identityService;
            _logInWindow = logInWindow;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            VersionDescription = GetVersionDescription();
            Theme = _themeSelectorService.GetCurrentTheme();
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            User = _userDataService.GetUser();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            UnregisterEvents();
        }

        private void UnregisterEvents()
        {
            _identityService.LoggedOut -= OnLoggedOut;
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private string GetVersionDescription()
        {
            var appName = "WpfPrismForcedLogin";
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);
            return $"{appName} - {versionInfo.FileVersion}";
        }

        private void OnSetTheme(string themeName)
        {
            var theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
            _themeSelectorService.SetTheme(theme);
        }

        private void OnPrivacyStatement()
        {
            // There is an open Issue on this
            // https://github.com/dotnet/corefx/issues/10361
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = _config.PrivacyStatement,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
            => true;

        private async void OnLogout()
        {
            await _identityService.LogoutAsync();
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            UnregisterEvents();
            var navigationService = _regionManager.Regions[Regions.Main].NavigationService;
            do
            {
                navigationService.Journal.GoBack();
            }
            while (navigationService.Journal.CanGoBack);
        }
    }
}
