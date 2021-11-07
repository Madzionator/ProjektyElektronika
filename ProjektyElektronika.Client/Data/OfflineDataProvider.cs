using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjektyElektronika.Shared.DTO;

namespace ProjektyElektronika.Client.Data
{
    class OfflineDataProvider
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

        public void SaveProjectList(List<ProjectDto> projects)
        {
            var json = JsonConvert.SerializeObject(projects, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public async Task AddProjectToList(ProjectDto project)
        {
            var projects = await GetProjectList();
            projects.Add(project);
            SaveProjectList(projects);
        }

        public async Task OpenProject(ProjectDto project)
        {
            var fullPath = new FileInfo(project.Address).DirectoryName;
            System.Diagnostics.Process.Start(fullPath);
        }
    }
}