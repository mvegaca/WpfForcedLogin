using System.Windows.Controls;

using WpfBasicOptionalLogin.ViewModels;

namespace WpfBasicOptionalLogin.Views
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
