using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

using Microsoft.Extensions.Options;

using WpfBasicForcedLogin.Contracts.Services;
using WpfBasicForcedLogin.Contracts.ViewModels;
using WpfBasicForcedLogin.Core.Contracts.Services;
using WpfBasicForcedLogin.Helpers;
using WpfBasicForcedLogin.Models;

namespace WpfBasicForcedLogin.ViewModels
{
    public class SettingsViewModel : Observable, INavigationAware
    {
        private readonly AppConfig _config;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly IUserDataService _userDataService;
        private readonly IIdentityService _identityService;
        private AppTheme _theme;
        private string _versionDescription;
        private ICommand _setThemeCommand;
        private ICommand _privacyStatementCommand;
        private ICommand _logoutCommand;
        private UserViewModel _user;

        public AppTheme Theme
        {
            get { return _theme; }
            set { Set(ref _theme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { Set(ref _versionDescription, value); }
        }

        public UserViewModel User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }

        public ICommand SetThemeCommand => _setThemeCommand ?? (_setThemeCommand = new RelayCommand<string>(OnSetTheme));

        public ICommand PrivacyStatementCommand => _privacyStatementCommand ?? (_privacyStatementCommand = new RelayCommand(OnPrivacyStatement));

        public ICommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new RelayCommand(OnLogout));

        public SettingsViewModel(IOptions<AppConfig> config, IThemeSelectorService themeSelectorService, IUserDataService userDataService, IIdentityService identityService)
        {
            _config = config.Value;
            _themeSelectorService = themeSelectorService;
            _userDataService = userDataService;
            _identityService = identityService;
        }

        public void OnNavigatedTo(object parameter)
        {
            VersionDescription = GetVersionDescription();
            Theme = _themeSelectorService.GetCurrentTheme();
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            User = _userDataService.GetUser();
        }

        public void OnNavigatedFrom()
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
            var appName = "WpfBasicForcedLogin";
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
        }
    }
}
