using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjektyElektronika.Shared.DTO;

namespace ProjektyElektronika.Client.Data
{
    class OnlineDataProvider : IDataProvider
    {
        public async Task<List<ProjectDto>> GetProjectList()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001/");

            var response = await client.GetStringAsync("project");

            var projects = JsonConvert.DeserializeObject<List<ProjectDto>>(response);

            return projects;
        }

        public async Task DownloadProject(int projectId)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001/");
            var response = await client.GetAsync($"project/download-request/{projectId}");

            var filename = response.Headers.GetValues("filename").First();
            var path = $"data/{projectId}/{filename}";
            var file = new FileInfo(path);
            file.Directory.Create();
            using (var fs = new FileStream(path, FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
            }
        }
    }
}