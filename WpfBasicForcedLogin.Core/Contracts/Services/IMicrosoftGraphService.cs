﻿using System.Threading.Tasks;

using WpfBasicForcedLogin.Core.Models;

namespace WpfBasicForcedLogin.Core.Contracts.Services
{
    public interface IMicrosoftGraphService
    {
        Task<User> GetUserInfoAsync(string accessToken);

        Task<string> GetUserPhoto(string accessToken);
    }
}
