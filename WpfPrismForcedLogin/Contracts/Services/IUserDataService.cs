using System;
using WpfPrismForcedLogin.ViewModels;

namespace WpfPrismForcedLogin.Contracts.Services
{
    public interface IUserDataService
    {
        event EventHandler<UserViewModel> UserDataUpdated;

        void Initialize();

        UserViewModel GetUser();
    }
}
