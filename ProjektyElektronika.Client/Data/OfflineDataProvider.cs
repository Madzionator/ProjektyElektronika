using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjektyElektronika.Shared.DTO;

namespace ProjektyElektronika.Client.Data
{
    class OfflineDataProvider : IDataProvider
    {
        private const string path = "offlineData.json";
        public async Task<List<ProjectDto>> GetProjectList()
        {
            try
            {
                var json = File.ReadAllText(path);
                var projects = JsonConvert.DeserializeObject<List<ProjectDto>>(json);
                return projects;
            }
            catch
            {
                return new List<ProjectDto>();
            }
        }

        public Task DownloadProject(int projectId)
        {
            throw new NotImplementedException();
        }

        public void SaveProjectList(List<ProjectDto> projects)
        {
            var json = JsonConvert.SerializeObject(projects, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}