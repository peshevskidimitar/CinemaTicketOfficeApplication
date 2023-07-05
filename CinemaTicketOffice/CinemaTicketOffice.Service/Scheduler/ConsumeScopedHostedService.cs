using CinemaTicketOffice.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;

namespace CinemaTicketOffice.Service.Scheduler
{
    public class ConsumeScopedHostedService : IHostedService, IDisposable
    {

        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public ConsumeScopedHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IBackgroundEmailSender>();
                await scopedProcessingService.DoWork();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
