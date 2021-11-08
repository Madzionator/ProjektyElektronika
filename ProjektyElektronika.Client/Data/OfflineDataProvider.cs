using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjektyElektronika.Shared.DTO;

namespace ProjektyElektronika.Client.Data
{
    class OfflineDataProvider
    {
        private const string path = "offlineData.json";
        public List<ProjectDto> GetProjectList()
        {
            try
            {
                var json = File.ReadAllText(path);
                var projects = JsonConvert.DeserializeObject<List<ProjectDto>>(json);
                return projects;
            }
            catch
            {
                return new ();
            }
        }

        public void SaveProjectList(List<ProjectDto> projects)
        {
            var json = JsonConvert.SerializeObject(projects, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public async Task AddProjectToList(ProjectDto project)
        {
            var projects = GetProjectList();
            projects.Add(project);
            SaveProjectList(projects);
        }

        public async Task OpenProject(ProjectDto project)
        {
            project.Address ??= GetProjectList().FirstOrDefault(x => x.Id == project.Id)?.Address;
            var file = new FileInfo(project.Address);

            var startInfo = new ProcessStartInfo(file.FullName)
            {
                UseShellExecute = true
            };

            Process.Start(startInfo);
        }
    }
}