using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.Data
{
    class DataProvider : IDataProvider
    {
        private readonly ILogger<DataProvider> _logger;
        private OnlineDataProvider _onlineDataProvider = new();
        private OfflineDataProvider _offlineDataProvider = new();

        public DataProvider(IOnlineDetector onlineDetector, ILogger<DataProvider> logger)
        {
            _logger = logger;
            IsOnline = onlineDetector.IsOnline;
            onlineDetector.OnOnlineChanged += online => IsOnline = online;
        }

        public bool IsOnline { get; set; }

        public async Task<List<Project>> GetProjectList()
        {
            try
            {
                if (!IsOnline)
                {
                    throw new Exception("not online");
                }

                var dict = _offlineDataProvider.GetProjectList().ToDictionary(x=>x.Id, x=>x);
                var projects = await _onlineDataProvider.GetProjectList();
                _logger.LogDebug($"Retrieved {projects.Count} projects");
                foreach (var project in projects)
                {
                    if(!dict.ContainsKey(project.Id))
                        dict.Add(project.Id, project);
                }

                return dict.Values.ToList();
            }
            catch
            {
                return _offlineDataProvider.GetProjectList();
            }
        }

        public async Task<List<string>> GetCategoryList()
        {
            if (IsOnline)
            {
                return await _onlineDataProvider.GetCategories();
            }
            else
            {
                return new List<string>() { "XD" };
            }
        }

        public async Task DownloadProject(Project project)
        {
            try
            {
                await _onlineDataProvider.DownloadProject(project);
                await _offlineDataProvider.AddProjectToList(project);
            }
            catch
            {

            }
        }

        public async Task OpenProject(Project project)
        {
            await _offlineDataProvider.OpenProject(project);
        }

        public async Task DeleteProject(Project project)
        {
            await _offlineDataProvider.DeleteProject(project);
            await _onlineDataProvider.DeleteProject(project);
        }
    }
}
