using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Identity.Client.Extensions.Msal;
using WpfBasicForcedLogin.Core.Contracts.Services;
using WpfLightForcedLogin.Contracts.Services;
using WpfLightForcedLogin.Contracts.Views;
using WpfLightForcedLogin.Models;
using WpfLightForcedLogin.ViewModels;

namespace WpfLightForcedLogin.Services
{
    public class ApplicationHostService : IApplicationHostService
    {
        private readonly INavigationService _navigationService;
        private readonly IPersistAndRestoreService _persistAndRestoreService;
        private readonly IIdentityService _identityService;
        private readonly IUserDataService _userDataService;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly AppConfig _config;

        private IShellWindow _shellWindow;
        private ILogInWindow _logInWindow;

        public ApplicationHostService(INavigationService navigationService, IThemeSelectorService themeSelectorService, IPersistAndRestoreService persistAndRestoreService, IIdentityService identityService, IUserDataService userDataService, AppConfig config)
        {
            _navigationService = navigationService;
            _themeSelectorService = themeSelectorService;
            _persistAndRestoreService = persistAndRestoreService;
            _identityService = identityService;
            _userDataService = userDataService;
            _config = config;
        }

        public async Task StartAsync()
        {
            // Initialize services that you need before app activation
            await InitializeAsync();

            // https://aka.ms/msal-net-token-cache-serialization
            var storageCreationProperties = new StorageCreationPropertiesBuilder(_config.IdentityCacheFileName, _config.IdentityCacheDirectoryName, _config.IdentityClientId).Build();
            var cacheHelper = await MsalCacheHelper.CreateAsync(storageCreationProperties).ConfigureAwait(false);
            _identityService.InitializeWithAadAndPersonalMsAccounts(_config.IdentityClientId, "http://localhost", cacheHelper);
            var silentLoginSuccess = await _identityService.AcquireTokenSilentAsync();
            if (!silentLoginSuccess || !_identityService.IsAuthorized())
            {
                _logInWindow = SimpleIoc.Default.GetInstance<ILogInWindow>();
                _logInWindow.ShowWindow();
                await StartupAsync();
                return;
            }

            _shellWindow = SimpleIoc.Default.GetInstance<IShellWindow>();
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _shellWindow.ShowWindow();
            _navigationService.NavigateTo(typeof(MainViewModel).FullName);

            // Tasks after activation
            await StartupAsync();
        }

        public async Task StopAsync()
        {
            await Task.CompletedTask;
            _identityService.LoggedIn -= OnLoggedIn;
            _identityService.LoggedOut -= OnLoggedOut;
            _persistAndRestoreService.PersistData();
        }

        private async Task InitializeAsync()
        {
            await Task.CompletedTask;
            _persistAndRestoreService.RestoreData();
            _themeSelectorService.SetTheme();
            _userDataService.Initialize();
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
        }

        private async Task StartupAsync()
        {
            await Task.CompletedTask;
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            _shellWindow = SimpleIoc.Default.GetInstance<IShellWindow>(Guid.NewGuid().ToString());
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _shellWindow.ShowWindow();
            _navigationService.NavigateTo(typeof(MainViewModel).FullName);
            _logInWindow.CloseWindow();
            _logInWindow = null;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            // Show the LogIn Window
            _logInWindow = SimpleIoc.Default.GetInstance<ILogInWindow>(Guid.NewGuid().ToString());
            _logInWindow.ShowWindow();

            // Close the Shell Window and
            _shellWindow.CloseWindow();
            _navigationService.UnsubscribeNavigation();
        }
    }
}
