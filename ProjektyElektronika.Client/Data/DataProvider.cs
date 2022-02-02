using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.Data
{
    class DataProvider : IDataProvider
    {
        private OnlineDataProvider _onlineDataProvider = new();
        private OfflineDataProvider _offlineDataProvider = new();

        public DataProvider(IOnlineDetector onlineDetector)
        {
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
                return new List<string>() { };
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
