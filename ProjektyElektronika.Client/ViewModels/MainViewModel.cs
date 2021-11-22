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

        private bool _isOnline = false;
        public bool IsOnline
        {
            get => _isOnline;
            set => SetProperty(ref _isOnline, value);
        }

        private List<Project> _projects = new();
        public List<Project> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }

        public ICommand DownloadProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand OpenAddProjectWindowCommand { get; }

        private void OpenAddProjectWindow()
        {
            var viewModel = new AddProjectViewModel();
            var view = new AddProjectWindow(viewModel);
            view.ShowDialog();
            LoadList();
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

        private async Task LoadList()
        {
            IsBusy = true;
            Projects = await _dataProvider.GetProjectList();
            IsBusy = false;
        }
    }
}