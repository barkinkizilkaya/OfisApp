using OfisApp.Controllers;
using OfisApp.Interfaces;
using System.Runtime.CompilerServices;

namespace OfisApp.Services
{
    public class OfficeBackgroundService : BackgroundService
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDeviceManager _deviceManager;

        public OfficeBackgroundService(ILogger<HomeController> logger, IDeviceManager deviceManager)
        {
            _logger = logger;
            _deviceManager = deviceManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Starting");

            stoppingToken.Register(() =>
                _logger.LogDebug("Stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                await _deviceManager.ReadDeviceData();
                await   Task.Delay(TimeSpan.FromHours(4), stoppingToken);
            }

            _logger.LogDebug($"Stopping.");
        }
    }
}
