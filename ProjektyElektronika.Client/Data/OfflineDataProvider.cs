using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.Data
{
    class OfflineDataProvider
    {
        private const string path = "offlineData.json";
        public List<Project> GetProjectList()
        {
            try
            {
                var json = File.ReadAllText(path);
                var projects = JsonConvert.DeserializeObject<List<Project>>(json);
                return projects;
            }
            catch
            {
                return new ();
            }
        }

        public void SaveProjectList(List<Project> projects)
        {
            var json = JsonConvert.SerializeObject(projects, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public async Task AddProjectToList(Project project)
        {
            var projects = GetProjectList();
            projects.Add(project);
            SaveProjectList(projects);
        }

        public async Task OpenProject(Project project)
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