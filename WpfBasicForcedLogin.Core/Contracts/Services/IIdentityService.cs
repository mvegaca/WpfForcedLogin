using System;
using System.Threading.Tasks;
using WpfBasicForcedLogin.Core.Helpers;

namespace WpfBasicForcedLogin.Core.Contracts.Services
{
    public interface IIdentityService
    {
        event EventHandler LoggedIn;

        event EventHandler LoggedOut;

        void InitializeWithAadAndPersonalMsAccounts();

        void InitializeWithAadMultipleOrgs(bool integratedAuth = false);

        void InitializeWithAadSingleOrg(string tenant, bool integratedAuth = false);

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
