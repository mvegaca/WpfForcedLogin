using System.Collections.Generic;
using System.Threading.Tasks;

using WpfBasicForcedLogin.Core.Models;

namespace WpfBasicForcedLogin.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync();
    }
}
