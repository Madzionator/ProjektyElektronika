using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjektyElektronika.Client.ViewModels;

namespace ProjektyElektronika.Client.Views
{
    /// <summary>
    /// Interaction logic for AddProjectWindow.xaml
    /// </summary>
    public partial class AddProjectWindow : Window
    {
        public AddProjectWindow(AddProjectViewModel viewModel)
        {
            InitializeComponent();
            viewModel.CloseWindow = this.Close;
            DataContext = viewModel;
        }
    }
}
