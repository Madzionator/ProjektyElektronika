using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
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
        public AddProjectViewModel()
        {
            AddProjectCommand = new AsyncCommand(AddProject);
            SelectFileCommand = new Command(SelectFile);
            AddAuthorCommand = new Command(() =>
            {
                Project.Authors.Add(new Author());
            });
            SubAuthorCommand = new Command(() => 
            {
                if(Project.Authors.Count > 0)
                    Project.Authors.RemoveAt(Project.Authors.Count - 1);
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

                Project.DateCreated ??= file.CreationTime;
            }
        }

        private async Task AddProject()
        {
            var fileuploader = new ProjectUploader();
            await fileuploader.UploadProject(Project);
            CloseWindow();
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
        public ICommand SubAuthorCommand { get; }
        public Action CloseWindow { get; set; }
    }
}
