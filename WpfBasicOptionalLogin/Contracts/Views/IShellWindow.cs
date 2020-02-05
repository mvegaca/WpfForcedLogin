using System.Windows.Controls;

namespace WpfBasicOptionalLogin.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();
    }
}
