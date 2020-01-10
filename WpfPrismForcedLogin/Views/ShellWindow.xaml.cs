using MahApps.Metro.Controls;

using Prism.Regions;

using WpfPrismForcedLogin.Constants;

namespace WpfPrismForcedLogin.Views
{
    public partial class ShellWindow : MetroWindow
    {
        public ShellWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionName(hamburgerMenuContentControl, Regions.Main);
            RegionManager.SetRegionManager(hamburgerMenuContentControl, regionManager);
        }
    }
}
