using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Client.Models;
using ProjektyElektronika.Client.Views;

namespace ProjektyElektronika.Client.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private readonly OnlineDetector _onlineDetector;
        private readonly IDataProvider _dataProvider;

        public AdminViewModel(OnlineDetector onlineDetector, IDataProvider dataProvider)
        {
            _onlineDetector = onlineDetector;
            _dataProvider = dataProvider;
            onlineDetector.OnOnlineChanged += isOnline => IsOnline = isOnline;

            AddProjectCommand = new Command(OpenAddProjectWindow);
            LoadData();
        }

        private void OpenAddProjectWindow()
        {
            var viewModel = new AddProjectViewModel(_onlineDetector, _dataProvider) { IsOnline = IsOnline };
            //var view = new AddProjectView(viewModel);
            //view.ShowDialog();
        }

        private async Task LoadData()
        {
            var categories = await _dataProvider.GetCategoryList();
            categories.Sort();
            Categories = categories;
        }

        private bool _isOnline = false;
        public bool IsOnline
        {
            get => _isOnline;
            set => SetProperty(ref _isOnline, value);
        }

        private List<string> _categories;
        public List<string> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public ICommand AddProjectCommand { get; }
        public Action CloseWindow { get; set; }
    }
}
