using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektyElektronika.Shared.DTO;

namespace ProjektyElektronika.Client.Data
{
    class DataProvider : IDataProvider
    {
        private OnlineDataProvider _onlineDataProvider = new();
        private OfflineDataProvider _offlineDataProvider = new();
     
        public async Task<List<ProjectDto>> GetProjectList()
        {
            try
            {
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

        public async Task DownloadProject(ProjectDto project)
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

        public async Task OpenProject(ProjectDto project)
        {
            await _offlineDataProvider.OpenProject(project);
        }
    }
}
