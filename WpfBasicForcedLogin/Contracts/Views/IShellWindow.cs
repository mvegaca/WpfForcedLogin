using System.ComponentModel;
using System.Windows.Controls;

namespace WpfBasicForcedLogin.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();
    }
}
