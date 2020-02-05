using System.Windows.Controls;

using WpfBasicForcedLogin.ViewModels;

namespace WpfBasicForcedLogin.Views
{
    public partial class MasterDetailPage : Page
    {
        public MasterDetailPage(MasterDetailViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
