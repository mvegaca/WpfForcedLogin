using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfBasicForcedLogin.Core.Contracts.Services
{
    public interface IIdentityCacheService
    {
        void SaveMsalToken(byte[] token);

        byte[] ReadMsalToken();
    }
}
