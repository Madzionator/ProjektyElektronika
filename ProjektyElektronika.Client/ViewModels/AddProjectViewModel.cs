using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.ViewModels
{
    class AddProjectViewModel : BaseViewModel
    {
        public AddProjectViewModel()
        {
            AddProjectCommand = new AsyncCommand(AddProject);
            SelectFileCommand = new Command(SelectFile);
            AddAuthorCommand = new Command(() =>
            {
                Project.Authors.Add(new Author());
            });
        }

        private void SelectFile()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var file = new FileInfo(openFileDialog.FileName);
                Project.Address = openFileDialog.FileName;
                FileName = file.Name;
                Project.DateCreated = file.CreationTime;
            }
        }

        private async Task AddProject()
        {
            var xd = new MultipartFormDataContent();
            //xd.Add("name", Project.)

            //xd.Add("file", Project.Address);
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
        public ICommand AddAuthorCommand { get; }
    }
}
