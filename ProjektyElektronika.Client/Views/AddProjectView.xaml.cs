using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProjektyElektronika.Client.ViewModels;

namespace ProjektyElektronika.Client.Views
{
    /// <summary>
    /// Interaction logic for AddProjectWindow.xaml
    /// </summary>
    public partial class AddProjectView : UserControl
    {
        public AddProjectView()
        {
            InitializeComponent();
        }

        private void AddProjectView_OnDrop(object sender, DragEventArgs e)
        {
            var vm = (AddProjectViewModel)DataContext;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            vm.SetFile(files?.FirstOrDefault());
        }
    }
}
