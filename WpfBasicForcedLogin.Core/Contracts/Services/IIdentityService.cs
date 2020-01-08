using System;
using System.Threading.Tasks;
using WpfBasicForcedLogin.Core.Helpers;

namespace WpfBasicForcedLogin.Core.Contracts.Services
{
    public interface IIdentityService
    {
        event EventHandler LoggedIn;

        event EventHandler LoggedOut;

        Task InitializeWithAadAndPersonalMsAccountsAsync(string redirectUri = null);

        Task InitializeWithAadMultipleOrgsAsync(bool integratedAuth = false, string redirectUri = null);

        Task InitializeWithAadSingleOrgAsync(string tenant, bool integratedAuth = false, string redirectUri = null);

        bool IsLoggedIn();

        Task<LoginResultType> LoginAsync();

        bool IsAuthorized();

        string GetAccountUserName();

        Task LogoutAsync();

        Task<string> GetAccessTokenForGraphAsync();

        Task<string> GetAccessTokenAsync(string[] scopes);

        Task<bool> AcquireTokenSilentAsync();
    }
}
