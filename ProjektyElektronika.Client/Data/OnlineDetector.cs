using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ProjektyElektronika.Client.Data
{
    public class OnlineDetector : IHostedService
    {
        public event OnOnlineChangedHandler OnOnlineChanged;
        public delegate void OnOnlineChangedHandler(bool isOnline);

        private bool _isOnline = false;
        private Timer _timer;
        private HttpClient _client;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(5));
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.Timeout = TimeSpan.FromSeconds(1);
        }

        private void DoWork(object? state)
        {
            var isOnlineNow = CheckOnline();

            if (_isOnline != isOnlineNow)
            {
                _isOnline = isOnlineNow;
                OnOnlineChanged?.Invoke(isOnlineNow);
            }
        }

        private bool CheckOnline()
        {
            try
            {
                _ = _client.GetAsync("/").Result;
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();
        }
    }
}
