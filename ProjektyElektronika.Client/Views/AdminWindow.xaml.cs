using System.Windows;
using ProjektyElektronika.Client.ViewModels;

namespace ProjektyElektronika.Client.Views
{
    public partial class AdminWindow : Window
    {
        public AdminWindow(AdminViewModel viewModel)
        {
            InitializeComponent();
            viewModel.CloseWindow = this.Close;
            DataContext = viewModel;
        }
    }
}
