﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Shared.DTO;

namespace ProjektyElektronika.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDataProvider _dataProvider;

        public MainViewModel(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            DownloadProjectCommand = new AsyncCommand<ProjectDto>(DownloadProject);
            OpenProjectCommand = new AsyncCommand<ProjectDto>(OpenProject);
            LoadList();
        }

        private async Task DownloadProject(ProjectDto project)
        {
            await _dataProvider.DownloadProject(project);
            Projects = Projects.ToList();
        }

        private async Task OpenProject(ProjectDto project)
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
            Projects = await _dataProvider.GetProjectList();
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
        public ICommand DownloadProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
    }
}
