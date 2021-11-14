using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.Data
{
    class OnlineDataProvider
    {
        public async Task<List<Project>> GetProjectList()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001/");

            var response = await client.GetStringAsync("project");

            var projects = JsonConvert.DeserializeObject<List<Project>>(response);

            return projects;
        }

        public async Task DownloadProject(Project project)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001/");
            var response = await client.GetAsync($"project/download-request/{project.Id}");

            var filename = response.Headers.GetValues("filename").First();
            var path = $"data/{project.Id}/{filename}";

            project.Address = path;
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