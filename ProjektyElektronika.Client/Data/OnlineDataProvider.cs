using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.Data
{
    class OnlineDataProvider
    {
        public async Task<List<Project>> GetProjectList()
        {
            var response = await WebHelpers.CreateClient().GetStringAsync("projects");
            var projects = JsonConvert.DeserializeObject<List<Project>>(response);
            return projects;
        }

        public async Task<List<string>> GetCategories()
        {
            var response = await WebHelpers.CreateClient().GetStringAsync("categories");
            var categories = JsonConvert.DeserializeObject<List<string>>(response);
            return categories;
        }

        public async Task DownloadProject(Project project)
        {
            var response = await WebHelpers.CreateClient().GetAsync($"file/{project.Id}");

            var filename = project.DownloadName;
            var path = $"data/{project.Id}/{filename}";

            project.LocalAddress = path;
            var file = new FileInfo(path);

            if (file.Directory.Exists)
            {
                file.Directory.Delete(true);
            }
            file.Directory.Create();

            using (var fs = new FileStream(path, FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
            }

            project.IsDownloaded = true;
        }
    }
}