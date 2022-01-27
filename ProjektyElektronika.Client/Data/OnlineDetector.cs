using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ProjektyElektronika.Client.Data
{
    public delegate void OnOnlineChangedHandler(bool isOnline);

    public interface IOnlineDetector
    {
        public bool IsOnline { get; }
        public event OnOnlineChangedHandler OnOnlineChanged;
    }

    public class OnlineDetector : IHostedService, IOnlineDetector
    {
        private Timer _timer;

        private bool _isOnline = false;
        public bool IsOnline => _isOnline;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(5));
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
                _ = WebHelpers.CreateClient().GetAsync("/").Result;
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

        public event OnOnlineChangedHandler OnOnlineChanged;
    }
}
