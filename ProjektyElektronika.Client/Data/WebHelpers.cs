using System;
using System.Net.Http;

namespace ProjektyElektronika.Client.Data
{
    public static class WebHelpers
    {
        public static HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://catalogue-project-backend.herokuapp.com/");
            return client;
        }
    }
}