using WpfBasicForcedLogin.Models;

namespace WpfBasicForcedLogin.Contracts.Services
{
    public interface IThemeSelectorService
    {
        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();
    }
}
