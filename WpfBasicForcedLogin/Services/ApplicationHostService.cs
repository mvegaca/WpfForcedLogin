using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using WpfBasicForcedLogin.Contracts.Services;
using WpfBasicForcedLogin.Contracts.Views;
using WpfBasicForcedLogin.Core.Contracts.Services;
using WpfBasicForcedLogin.ViewModels;

namespace WpfBasicForcedLogin.Services
{
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigationService;
        private readonly IPersistAndRestoreService _persistAndRestoreService;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly IIdentityService _identityService;
        private readonly IUserDataService _userDataService;

        private IShellWindow _shellWindow;
        private ILogInWindow _logInWindow;

        public ApplicationHostService(IServiceProvider serviceProvider, INavigationService navigationService, IThemeSelectorService themeSelectorService, IPersistAndRestoreService persistAndRestoreService, IIdentityService identityService, IUserDataService userDataService)
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            _identityService = identityService;
            _userDataService = userDataService;
            _shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _themeSelectorService = themeSelectorService;
            _persistAndRestoreService = persistAndRestoreService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Initialize services that you need before app activation
            await InitializeAsync();
            _userDataService.Initialize();
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            _identityService.InitializeWithAadAndPersonalMsAccounts();
            var silentLoginSuccess = await _identityService.AcquireTokenSilentAsync();
            if (!silentLoginSuccess || !_identityService.IsAuthorized())
            {
                _logInWindow = _serviceProvider.GetService(typeof(ILogInWindow)) as ILogInWindow;
                _logInWindow.ShowWindow();
                await StartupAsync();
                return;
            }

            _shellWindow.ShowWindow();
            _navigationService.NavigateTo(typeof(MainViewModel).FullName);

            // Tasks after activation
            await StartupAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            _persistAndRestoreService.PersistData();
        }

        private async Task InitializeAsync()
        {
            await Task.CompletedTask;
            _persistAndRestoreService.RestoreData();
            _themeSelectorService.SetTheme();
        }

        private async Task StartupAsync()
        {
            await Task.CompletedTask;
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            _shellWindow.ShowWindow();
            _navigationService.NavigateTo(typeof(MainViewModel).FullName);
            _logInWindow.CloseWindow();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            // Show the LogIn Window
            _logInWindow = _serviceProvider.GetService(typeof(ILogInWindow)) as ILogInWindow;
            _logInWindow.ShowWindow();

            // Close the Shell Window and
            _shellWindow.CloseWindow();
            _navigationService.UnregisterNavigation();

            _shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
        }
    }
}
