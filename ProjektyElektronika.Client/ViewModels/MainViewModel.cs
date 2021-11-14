using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Client.Models;
using ProjektyElektronika.Client.Views;

namespace ProjektyElektronika.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDataProvider _dataProvider;

        public MainViewModel(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            DownloadProjectCommand = new AsyncCommand<Project>(DownloadProject);
            OpenProjectCommand = new AsyncCommand<Project>(OpenProject);
            OpenAddProjectWindowCommand = new Command(OpenAddProjectWindow);
            LoadList();
        }

        private void OpenAddProjectWindow()
        {
            var viewModel = new AddProjectViewModel();
            var view = new AddProjectWindow
            {
                DataContext = viewModel
            };
            view.ShowDialog();
        }

        private async Task DownloadProject(Project project)
        {
            await _dataProvider.DownloadProject(project);
            Projects = Projects.ToList();
        }

        private async Task OpenProject(Project project)
        {
            try
            {
                await _dataProvider.OpenProject(project);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd. Treść błędu: {ex}");
            }

        }

        private void UseFilter()
        {
            if (_filterString.Length < 1)
            {
                FilteredProjects = Projects.ToList();
                return;
            }

            bool Filter(Project project)
            {
                if (project.Title.Contains(FilterString, StringComparison.InvariantCultureIgnoreCase))
                    return true;
                if (project.Authors.Any(author => author.Name.Contains(FilterString, StringComparison.InvariantCultureIgnoreCase)))
                    return true;
                if (project.Authors.Any(author => author.Index.ToString().Contains(FilterString, StringComparison.InvariantCultureIgnoreCase)))
                    return true;
                if (project.DateCreated.Value.ToString("dd MMMM yyyy")
                    .Contains(FilterString, StringComparison.InvariantCultureIgnoreCase))
                    return true;
                return false;
            }

            FilteredProjects = Projects.Where(Filter).ToList();
        }

       

        private async Task LoadList()
        {
            Projects = await _dataProvider.GetProjectList();
            FilteredProjects = Projects.ToList();
        }

        private List<Project> _projects = new();
        public List<Project> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }

        private List<Project> _filteredProjects = new();
        public List<Project> FilteredProjects
        {
            get => _filteredProjects;
            set => SetProperty(ref _filteredProjects, value);
        }

        private string _filterString;
        public string FilterString
        {
            get => _filterString;
            set => SetProperty(ref _filterString, value, onChanged:UseFilter);
        }

        public ICommand DownloadProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand OpenAddProjectWindowCommand { get; }
    }
}
