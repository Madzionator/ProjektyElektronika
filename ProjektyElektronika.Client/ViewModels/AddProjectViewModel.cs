using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
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
        public AddProjectViewModel(OnlineDetector onlineDetector)
        {
            onlineDetector.OnOnlineChanged += isOnline => IsOnline = isOnline;

            AddProjectCommand = new AsyncCommand(AddProject);
            SelectFileCommand = new Command(SelectFile);
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

        private Project _project = new ();
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

        public ICommand AddProjectCommand { get; }
        public ICommand SelectFileCommand { get; }
        public Action CloseWindow { get; set; }
    }
}
