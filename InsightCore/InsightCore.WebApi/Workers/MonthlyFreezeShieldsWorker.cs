using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using PyrosFit.Application.Features.Streaks.Commands;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.WebApi.Workers
{
    public class MonthlyFreezeShieldsWorker : BackgroundService
    {
        private readonly ILogger<MonthlyFreezeShieldsWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeZoneInfo _timeZone;
        private readonly int _runHour;
        private readonly int _runMinute;

        public MonthlyFreezeShieldsWorker(ILogger<MonthlyFreezeShieldsWorker> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            var tzId = configuration["MonthlyReset:TimeZone"] ?? "UTC";
            try { _timeZone = TimeZoneInfo.FindSystemTimeZoneById(tzId); }
            catch { _timeZone = TimeZoneInfo.Utc; }

            _runHour = int.TryParse(configuration["MonthlyReset:RunHour"], out var h) ? h : 0;
            _runMinute = int.TryParse(configuration["MonthlyReset:RunMinute"], out var m) ? m : 5;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MonthlyFreezeShieldsWorker started (TZ: {tz}, runAt {h}:{m})", _timeZone.Id, _runHour, _runMinute);

            var rand = new Random();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var nowUtc = DateTimeOffset.UtcNow;
                    var localNow = TimeZoneInfo.ConvertTime(nowUtc, _timeZone);

                    // Calculate target: last day of current month at configured hour:minute (local time)
                    var lastDay = DateTime.DaysInMonth(localNow.Year, localNow.Month);
                    var targetLocal = new DateTime(localNow.Year, localNow.Month, lastDay, _runHour, _runMinute, 0);
                    var targetUtc = TimeZoneInfo.ConvertTimeToUtc(targetLocal, _timeZone);

                    // If already passed, move to next month's last day
                    if (targetUtc <= DateTimeOffset.UtcNow)
                    {
                        var nextMonth = localNow.AddMonths(1);
                        var nextLastDay = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
                        targetLocal = new DateTime(nextMonth.Year, nextMonth.Month, nextLastDay, _runHour, _runMinute, 0);
                        targetUtc = TimeZoneInfo.ConvertTimeToUtc(targetLocal, _timeZone);
                    }

                    var delay = targetUtc - DateTimeOffset.UtcNow;
                    if (delay > TimeSpan.Zero)
                    {
                        _logger.LogInformation("Next run scheduled at {targetLocal} (UTC {targetUtc}), sleeping {delay}", targetLocal, targetUtc, delay);
                        await Task.Delay(delay, stoppingToken);
                    }

                    // Short random jitter to avoid thundering herd when multiple instances start at same time
                    var jitterSeconds = rand.Next(0, 31);
                    if (jitterSeconds > 0)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(jitterSeconds), stoppingToken);
                    }

                    // Build period based on targetLocal (which corresponds to the month being closed)
                    var period = targetLocal.ToString("yyyy-MM", CultureInfo.InvariantCulture);
                    _logger.LogInformation("Triggering reset for period {period} (local target {targetLocal})", period, targetLocal);

                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        await mediator.Send(new ResetMonthlyFreezeShieldsCommand(period), stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error executing ResetMonthlyFreezeShieldsCommand for {period}", period);
                    }

                    // After execution, loop continues and will calculate next month's target
                }
                catch (TaskCanceledException)
                {
                    // Graceful shutdown
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in MonthlyFreezeShieldsWorker loop");
                    // Wait a short time before retrying to avoid tight error loop
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }
        }
    }
}
