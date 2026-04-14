using FinCouple.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FinCouple.Infrastructure.HostedServices;

public class RecurrenceHostedService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RecurrenceHostedService> _logger;
    private Timer? _timer;

    public RecurrenceHostedService(IServiceProvider serviceProvider, ILogger<RecurrenceHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var nextMidnight = now.Date.AddDays(1);
        var delay = nextMidnight - now;
        _timer = new Timer(ProcessRecurrences, null, delay, TimeSpan.FromDays(1));
        return Task.CompletedTask;
    }

    private void ProcessRecurrences(object? state)
    {
        _ = ProcessRecurrencesAsync();
    }

    private async Task ProcessRecurrencesAsync()
    {
        _logger.LogInformation("Processing due recurring expenses at {Time}", DateTime.UtcNow);
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IRecurringExpenseService>();
            await service.ProcessDueRecurrencesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing recurring expenses");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose() => _timer?.Dispose();
}
