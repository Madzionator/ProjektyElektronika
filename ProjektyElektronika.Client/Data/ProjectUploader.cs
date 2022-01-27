using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.Data
{
    class ProjectUploader
    {


        public async Task UploadProject(Project project)
        {
            var file = new FileInfo(project.LocalAddress);
            var fileContent = new StreamContent(file.OpenRead())
            {
                Headers =
                {
                    ContentLength = file.Length,
                }
            };

            var formDataContent = new MultipartFormDataContent();
            formDataContent.Add(fileContent, "file", file.Name);          // file

            var json = JsonConvert.SerializeObject(project);
            formDataContent.Add(new StringContent(json), "json");   // form input

            var client = WebHelpers.CreateClient();

            var response = await client.PostAsync("admin/add_mp", formDataContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Błąd serwera {response.StatusCode:D}: {response.StatusCode}");
            }
        }

        public async Task AddCategory(string category)
        {
            var client = WebHelpers.CreateClient();
            await client.PostAsync($"admin/add_category/{category}", new StringContent(string.Empty));
        }
    }
}
