using WpfPrismForcedLogin.Models;

namespace WpfPrismForcedLogin.Contracts.Services
{
    public interface IThemeSelectorService
    {
        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();
    }
}
