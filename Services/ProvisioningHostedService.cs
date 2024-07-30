using CarWashManagementSystem.Data;
using CarWashManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagementSystem.Services
{
    public class ProvisioningHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ProvisioningHostedService> _logger;
        private DateTime? _lastTurnOnExecution;
        private DateTime? _lastTurnOffExecution;
        private DateTime? _lastNoScheduleLog;
        private DateTime? _lastSkipLog;

        public ProvisioningHostedService(IServiceScopeFactory scopeFactory, ILogger<ProvisioningHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ProvisioningHostedService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                        var stationService = scope.ServiceProvider.GetRequiredService<IStationService>();

                        var now = DateTime.Now;
                        var currentDay = now.DayOfWeek;
                        var currentTime = new TimeSpan(now.Hour, now.Minute, 0); // Ignorowanie sekund

                        // Pobierz harmonogram dla bieżącego dnia
                        var schedule = await context.FiscSchedule
                            .Where(s => s.DayOfWeek == currentDay)
                            .FirstOrDefaultAsync(stoppingToken);

                        if (schedule != null)
                        {
                            // Pominięcie provisioning, jeśli TurnOnTime i TurnOffTime są równe 00:00:00
                            if (schedule.TurnOnTime == TimeSpan.Zero && schedule.TurnOffTime == TimeSpan.Zero)
                            {
                                if (!_lastSkipLog.HasValue || _lastSkipLog.Value.Date < now.Date)
                                {
                                    _logger.LogInformation($"Skipping provisioning for {currentDay} as both times are set to 00:00:00.");
                                    _lastSkipLog = now;
                                }
                            }
                            else
                            {
                                // Sprawdź, czy jest czas na włączenie provisioning
                                if (currentTime == schedule.TurnOnTime &&
                                    (!_lastTurnOnExecution.HasValue || _lastTurnOnExecution.Value.Date < now.Date))
                                {
                                    _logger.LogInformation($"Executing provisioning at turn on time {now}");
                                    await stationService.ProvisionActiveStationsAsync();
                                    _lastTurnOnExecution = now;
                                }

                                // Sprawdź, czy jest czas na wyłączenie provisioning
                                if (currentTime == schedule.TurnOffTime &&
                                    (!_lastTurnOffExecution.HasValue || _lastTurnOffExecution.Value.Date < now.Date))
                                {
                                    _logger.LogInformation($"Executing provisioning at turn off time {now}");
                                    await stationService.ProvisionActiveStationsAsync();
                                    _lastTurnOffExecution = now;
                                }
                            }
                        }
                        else
                        {
                            if (!_lastNoScheduleLog.HasValue || _lastNoScheduleLog.Value.Date < now.Date)
                            {
                                _logger.LogInformation($"No schedule found for {currentDay}");
                                _lastNoScheduleLog = now;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during provisioning: {ex.Message}");
                }

                // Odczekaj 1 minutę przed kolejnym sprawdzeniem
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("ProvisioningHostedService is stopping.");
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ProvisioningHostedService is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}
