using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjektyElektronika.Client.Data
{
    public static class WebHelpers
    {
        private const string Login = "admin";
        public static string Password { get; set; }
        public static string AuthToken => Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Login}:{Password}"));

        public static HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://kni.prz.edu.pl:5050/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthToken);
            return client;
        }

        public static async Task<bool> CheckAdmin()
        {
            try
            {
                var response = await CreateClient().GetAsync("/admin/check");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}