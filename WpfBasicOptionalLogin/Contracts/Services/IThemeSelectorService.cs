using WpfBasicOptionalLogin.Models;

namespace WpfBasicOptionalLogin.Contracts.Services
{
    public interface IThemeSelectorService
    {
        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();
    }
}
