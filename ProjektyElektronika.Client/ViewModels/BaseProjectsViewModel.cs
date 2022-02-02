using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FuzzySharp;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProjektyElektronika.Client.Data;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.ViewModels
{
    public abstract class BaseProjectListViewModel : BaseViewModel
    {
        private const int FuzzySearchThreshold = 60;
        private const int PageSize = 25;
        private readonly IDataProvider _dataProvider;

        protected BaseProjectListViewModel(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            ChangePageCommand = new Command<int>(ChangePage);
        }

        public ICommand ChangePageCommand { get; }

        private List<Project> _projects = new();
        public List<Project> AllProjects
        {
            get => _projects;
            set => SetProperty(ref _projects, value, onChanged: () =>
            {
                Page = 1;
                Filter();
            });
        }

        private List<Project> _filteredProjects = new();
        public List<Project> FilteredProjects
        {
            get => _filteredProjects;
            set => SetProperty(ref _filteredProjects, value);
        }

        private int _page = 1;
        public int Page
        {
            get => _page;
            set
            {
                var p = Math.Max(1, Math.Min(MaxPages, value));
                SetProperty(ref _page, p, onChanged: Filter);
            }
        }

        private int _maxPages = 1;
        public int MaxPages
        {
            get => _maxPages;
            set => SetProperty(ref _maxPages, value);
        }

        private string _searchString;
        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value, onChanged: () =>
            {
                Page = 1;
                Filter();
            });
        }

        private bool? _isDiploma = false;
        public bool? IsDiploma
        {
            get => _isDiploma;
            set => SetProperty(ref _isDiploma, value, onChanged: () =>
            {
                Page = 1;
                Filter();
            });
        }

        private void ChangePage(int obj)
        {
            Page += obj;
        }

        protected async Task LoadList()
        {
            IsBusy = true;
            AllProjects = await _dataProvider.GetProjectList();
            IsBusy = false;
        }

        protected void Filter()
        {
            bool? diploma = IsDiploma switch
            {
                true => true,
                false => null,
                null => false
            };

            var all = AllProjects
                .Where(x => diploma is null || diploma == x.IsDiploma)
                .Select(x => (project: x, ratio: Search(x, SearchString)))
                .OrderByDescending(x => x.ratio)
                .Where(x => x.ratio > FuzzySearchThreshold)
                .Select(x => x.project)
                .ToList();

            MaxPages = (int)Math.Ceiling(all.Count / (double)PageSize);
            FilteredProjects = all.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
        }

        private int Search(Project project, string filterString)
        {
            if (filterString is null or { Length: < 1 }) return 100;
            var searchString = $"{project.Title} | {project.AcademicYear} | {project.Author} | {project.Category}";
            var ratio = 0;
            if (searchString.Contains(filterString, StringComparison.CurrentCultureIgnoreCase))
                return 100;
            var m1 = Fuzz.WeightedRatio(searchString, filterString);
            var m2 = Fuzz.WeightedRatio(project.Title, filterString);
            var m3 = Fuzz.WeightedRatio(project.Author, filterString);
            var m4 = Fuzz.WeightedRatio(project.Category, filterString);
            return Math.Max(m1, Math.Max(m2, Math.Max(m3, m4)));
        }
    }
}