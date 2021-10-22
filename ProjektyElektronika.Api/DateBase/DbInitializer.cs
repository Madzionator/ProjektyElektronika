using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjektyElektronika.Api.Models;

namespace ProjektyElektronika.Api.DateBase
{
    public class DbInitializer : IHostedService
    {
        private readonly IServiceProvider _isp;

        public DbInitializer(IServiceProvider isp)
        {
            _isp = isp;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _isp.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    await context.Database.MigrateAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<DbInitializer>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            
        }
    }
}
