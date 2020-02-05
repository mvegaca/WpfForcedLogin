using System;

using WpfBasicForcedLogin.ViewModels;

namespace WpfBasicForcedLogin.Contracts.Services
{
    public interface IUserDataService
    {
        event EventHandler<UserViewModel> UserDataUpdated;

        void Initialize();

        UserViewModel GetUser();
    }
}
