using System;
using System.Collections.Generic;
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
    }
}