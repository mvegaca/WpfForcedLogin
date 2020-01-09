using WpfLightForcedLogin.Models;

namespace WpfLightForcedLogin.Contracts.Services
{
    public interface IThemeSelectorService
    {
        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();
    }
}
