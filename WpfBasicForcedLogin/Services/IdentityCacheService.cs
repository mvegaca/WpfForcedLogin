using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using WpfBasicForcedLogin.Core.Contracts.Services;

namespace WpfBasicForcedLogin.Services
{
    public class IdentityCacheService : IIdentityCacheService
    {
        private readonly object _fileLock = new object();
        private readonly string _msalCacheFilePath = Assembly.GetExecutingAssembly().Location + ".msalcache.bin3";        

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
