using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using WpfBasicForcedLogin.Core.Contracts.Services;

namespace WpfBasicForcedLogin.Services
{
    public class IdentityCacheService : IIdentityCacheService
    {
        public static readonly string _msalCacheFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{Assembly.GetExecutingAssembly().GetName().Name}\\.msalcache.bin3";
        private readonly object _fileLock = new object();

        public byte[] ReadMsalToken()
        {
            lock (_fileLock)
            {
                if (File.Exists(_msalCacheFilePath))
                {
                    var encryptedData = File.ReadAllBytes(_msalCacheFilePath);
                    return ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                }

                return default;
            }
        }

        public void SaveMsalToken(byte[] token)
        {
            lock (_fileLock)
            {
                var encryptedData = ProtectedData.Protect(token, null, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(_msalCacheFilePath, encryptedData);
            }
        } 
    }
}
