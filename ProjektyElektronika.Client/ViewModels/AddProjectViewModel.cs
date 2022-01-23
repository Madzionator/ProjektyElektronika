using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.ViewModels
{
    public class AddProjectViewModel : BaseViewModel
    {
        private readonly IDataProvider _dataProvider;

        public AddProjectViewModel(OnlineDetector onlineDetector, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            onlineDetector.OnOnlineChanged += isOnline => IsOnline = isOnline;

            AddProjectCommand = new AsyncCommand(AddProject);
            SelectFileCommand = new Command(SelectFile);
            LoadData();
        }

        private async Task LoadData()
        {
            var categories = await _dataProvider.GetCategoryList();
            categories.Sort();
            Categories = categories;
            SelectedCategory = categories.FirstOrDefault();
        }

        private bool _isOnline = false;
        public bool IsOnline
        {
            get => _isOnline;
            set => SetProperty(ref _isOnline, value);
        }

        private void SelectFile()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var file = new FileInfo(openFileDialog.FileName);
                Project.LocalAddress = openFileDialog.FileName;
                FileName = file.Name;
            }
        }

        private async Task AddProject()
        {
            try
            {
                var fileuploader = new ProjectUploader();
                await fileuploader.UploadProject(Project);
                CloseWindow();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Nie udało się dodać projektu.\n{ex.Message}", "Błąd");
            }
        }

        private Project _project = new() { AcademicYear = DateTime.Now.Year };
        public Project Project
        {
            get => _project;
            set => SetProperty(ref _project, value);
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        private List<string> _categories;
        public List<string> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value, onChanged: () => Project.Category = value);
        }

        public ICommand AddProjectCommand { get; }
        public ICommand SelectFileCommand { get; }
        public Action CloseWindow { get; set; }
    }
}
