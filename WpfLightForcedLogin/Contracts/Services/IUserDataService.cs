using System;
using WpfLightForcedLogin.ViewModels;

namespace WpfLightForcedLogin.Contracts.Services
{
    public interface IUserDataService
    {
        event EventHandler<UserViewModel> UserDataUpdated;

        void Initialize();

        UserViewModel GetUser();
    }
}
