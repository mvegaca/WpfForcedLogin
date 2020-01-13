using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Extensions.Msal;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using WpfBasicForcedLogin.Core.Contracts.Services;
using WpfBasicForcedLogin.Core.Services;
using WpfPrismForcedLogin.Constants;
using WpfPrismForcedLogin.Contracts.Services;
using WpfPrismForcedLogin.Models;
using WpfPrismForcedLogin.Services;
using WpfPrismForcedLogin.ViewModels;
using WpfPrismForcedLogin.Views;

namespace WpfPrismForcedLogin
{
    public partial class App : PrismApplication
    {
        private string[] _startUpArgs;
        private LogInWindow _logInWindow;

        public App()
        {
        }

        protected override Window CreateShell()
            => Container.Resolve<ShellWindow>();

        protected async override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.RestoreData();
            var themeSelectorService = Container.Resolve<IThemeSelectorService>();
            themeSelectorService.SetTheme();
            var identityService = Container.Resolve<IIdentityService>();
            identityService.LoggedIn += OnLoggedIn;
            identityService.LoggedOut += OnLoggedOut;
            var silentLoginSuccess = await identityService.AcquireTokenSilentAsync();
            if (!silentLoginSuccess || !identityService.IsAuthorized())
            {
                ShowLogInWindow();
            }

            var userDataService = Container.Resolve<IUserDataService>();
            userDataService.Initialize();
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
            _logInWindow.Close();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Hide();
            ShowLogInWindow();
        }

        private void ShowLogInWindow()
        {
            _logInWindow = Container.Resolve<LogInWindow>();
            _logInWindow.Closed += OnLogInWindowClosed;
            _logInWindow.ShowDialog();
        }

        private void OnLogInWindowClosed(object sender, EventArgs e)
        {
            if (sender is Window window)
            {
                window.Closed -= OnLogInWindowClosed;
                var identityService = Container.Resolve<IIdentityService>();
                if (!identityService.IsLoggedIn())
                {
                    Application.Current.Shutdown();
                }
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _startUpArgs = e.Args;
            base.OnStartup(e);
        }

        protected async override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Core Services
            containerRegistry.Register<IFilesService, FilesService>();
            containerRegistry.Register<IMicrosoftGraphService, MicrosoftGraphService>();

            // https://aka.ms/msal-net-token-cache-serialization
            var identityService = new IdentityService();
            var storageCreationProperties = new StorageCreationPropertiesBuilder(".msalcache.dat", "MSAL_CACHE", "31f2256a-e9aa-4626-be94-21c17add8fd9").Build();
            var cacheHelper = await MsalCacheHelper.CreateAsync(storageCreationProperties).ConfigureAwait(false);
            identityService.InitializeWithAadAndPersonalMsAccounts("31f2256a-e9aa-4626-be94-21c17add8fd9", "http://localhost", cacheHelper);
            containerRegistry.RegisterInstance<IIdentityService>(identityService);

            // App Services
            containerRegistry.Register<IPersistAndRestoreService, PersistAndRestoreService>();
            containerRegistry.Register<IThemeSelectorService, ThemeSelectorService>();
            containerRegistry.Register<IUserDataService, UserDataService>();

            // Views
            containerRegistry.RegisterForNavigation<ShellWindow>();

            containerRegistry.RegisterForNavigation<MainPage>(PageKeys.Main);

            containerRegistry.RegisterForNavigation<SettingsPage>(PageKeys.Settings);

            // Configuration
            var configuration = BuildConfiguration();
            var appConfig = configuration
                .GetSection(nameof(AppConfig))
                .Get<AppConfig>();

            // Register configurations to IoC
            containerRegistry.RegisterInstance<IConfiguration>(configuration);
            containerRegistry.RegisterInstance<AppConfig>(appConfig);
        }

        private IConfiguration BuildConfiguration()
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(appLocation)
                .AddJsonFile("appsettings.json")
                .AddCommandLine(_startUpArgs)
                .Build();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            // We are remapping the default ViewName and ViewNameViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelName = string.Format(CultureInfo.InvariantCulture, "WpfPrismForcedLogin.ViewModels.{0}ViewModel, WpfPrismForcedLogin", viewType.Name[0..^4]);
                return Type.GetType(viewModelName);
            });
            ViewModelLocationProvider.Register(typeof(LogInWindow).FullName, typeof(LogInViewModel));
            ViewModelLocationProvider.Register(typeof(ShellWindow).FullName, typeof(ShellViewModel));
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.PersistData();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0

            // e.Handled = true;
        }
    }
}
