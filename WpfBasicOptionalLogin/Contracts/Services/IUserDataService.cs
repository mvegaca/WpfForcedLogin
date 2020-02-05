using System;

using WpfBasicOptionalLogin.ViewModels;

namespace WpfBasicOptionalLogin.Contracts.Services
{
    public interface IUserDataService
    {
        event EventHandler<UserViewModel> UserDataUpdated;

        void Initialize();

        UserViewModel GetUser();
    }
}
