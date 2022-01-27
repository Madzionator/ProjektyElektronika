using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private readonly Navigation _navigation;
        private readonly IDataProvider _dataProvider;

        public AdminViewModel(Navigation navigation, IOnlineDetector onlineDetector, IDataProvider dataProvider)
        {
            _navigation = navigation;
            _dataProvider = dataProvider;

            IsOnline = onlineDetector.IsOnline;
            onlineDetector.OnOnlineChanged += delegate(bool isOnline)
            {
                IsOnline = isOnline;
                LoadData();
            };

            AddProjectCommand = new Command(OpenAddProjectWindow);
            GoBackCommand = new Command(GoBack);
            DeleteProjectCommand = new AsyncCommand<Project>(DeleteProject);

            LoadData();
        }

        private async Task DeleteProject(Project project)
        {
            IsBusy = true;
            try
            {
                await _dataProvider.DeleteProject(project);
                await LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie udało się usunąć projektu.\n{ex.Message}");
            }
            
            IsBusy = false;
        }

        private void GoBack()
        {
            _navigation.Navigate<HomeViewModel>();
        }

        private void OpenAddProjectWindow()
        {
            _navigation.Navigate<AddProjectViewModel>();
        }

        private async Task LoadData()
        {
            IsBusy = true;
            Projects = await _dataProvider.GetProjectList();
            IsBusy = false;
        }

        private bool _isOnline = false;
        public bool IsOnline
        {
            get => _isOnline;
            set => SetProperty(ref _isOnline, value);
        }

        private List<Project> _projects;
        public List<Project> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }

        public ICommand GoBackCommand { get; }
        public ICommand AddProjectCommand { get; }
        public ICommand DeleteProjectCommand { get; }
    }
}
