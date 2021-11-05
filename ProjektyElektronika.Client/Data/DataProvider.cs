using System;
using System.Collections.Generic;
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
                var projects = await _onlineDataProvider.GetProjectList();
                _offlineDataProvider.SaveProjectList(projects);
                return projects;
            }
            catch
            {
                return await _offlineDataProvider.GetProjectList();
            }
        }

        public async Task DownloadProject(int projectId)
        {
            try
            {
                await _onlineDataProvider.DownloadProject(projectId);
            }
            catch { }
        }
    }
}
