using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Shared.DTO;

namespace ProjektyElektronika.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            IncrementCommand = new AsyncCommand(Execute);
        }

        private async Task Execute()
        {
            var dataProvider = new DataProvider();
            Projects = await dataProvider.GetProjectList();
        }

        private List<ProjectDto> _projects = new();
        public List<ProjectDto> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }

        private int _n1;
        public int N1
        {
            get => _n1;
            set => SetProperty(ref _n1, value);
        }


        public ICommand IncrementCommand { get; }
    }
}
