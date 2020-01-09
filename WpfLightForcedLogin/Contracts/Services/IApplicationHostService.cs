using System.Threading.Tasks;

namespace WpfLightForcedLogin.Contracts.Services
{
    public interface IApplicationHostService
    {
        Task StartAsync();

        Task StopAsync();
    }
}
