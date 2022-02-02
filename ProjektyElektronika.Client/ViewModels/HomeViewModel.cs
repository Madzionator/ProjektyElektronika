using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Client.Models;
using ProjektyElektronika.Client.Views;

namespace ProjektyElektronika.Client.ViewModels
{
    public class HomeViewModel : BaseProjectListViewModel
    {
        private readonly IDataProvider _dataProvider;
        private readonly Navigation _navigation;

        public HomeViewModel(IDataProvider dataProvider, IOnlineDetector onlineDetector, Navigation navigation)
            : base(dataProvider)
        {
            _dataProvider = dataProvider;
            _navigation = navigation;
            DownloadProjectCommand = new AsyncCommand<Project>(DownloadProject);
            OpenProjectCommand = new AsyncCommand<Project>(OpenProject);
            OpenAdminWindowCommand = new Command(OpenAdminWindow);

            IsOnline = onlineDetector.IsOnline;
            onlineDetector.OnOnlineChanged += OnOnlineChanged;

            LoadList();
        }

        private void OnOnlineChanged(bool isonline)
        {
            IsOnline = isonline;
            LoadList();
        }

        private bool _isOnline = false;
        public bool IsOnline
        {
            get => _isOnline;
            set => SetProperty(ref _isOnline, value);
        }


        public ICommand DownloadProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand OpenAdminWindowCommand { get; }

        private void OpenAdminWindow()
        {
            _navigation.Navigate<LoginViewModel>();
        }

        private async Task DownloadProject(Project project)
        {
            await _dataProvider.DownloadProject(project);
            Filter();
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
    }
}