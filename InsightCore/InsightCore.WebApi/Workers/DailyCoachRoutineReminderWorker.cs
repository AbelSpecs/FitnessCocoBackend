using InsightCore.Application.Interface.Presentation;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InsightCore.WebApi.Workers
{
    public class DailyCoachRoutineReminderWorker : BackgroundService
    {
        private readonly ILogger<DailyCoachRoutineReminderWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeZoneInfo _timeZone;

        public DailyCoachRoutineReminderWorker(
            ILogger<DailyCoachRoutineReminderWorker> logger,
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            var tzId = configuration["CoachReminderWorker:TimeZone"] ?? "America/Caracas";
            try 
            { 
                _timeZone = TimeZoneInfo.FindSystemTimeZoneById(tzId); 
            }
            catch 
            {
                try 
                {
                    _timeZone = TimeZoneInfo.FindSystemTimeZoneById("Venezuela Standard Time");
                }
                catch
                {
                    _timeZone = TimeZoneInfo.CreateCustomTimeZone("VET", TimeSpan.FromHours(-4), "Venezuela Standard Time", "Venezuela Standard Time");
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DailyCoachRoutineReminderWorker iniciado con zona horaria: {tz}", _timeZone.DisplayName);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var nowUtc = DateTimeOffset.UtcNow;
                    var localNow = TimeZoneInfo.ConvertTime(nowUtc, _timeZone);

                    // Calcular las 20:00 hrs de hoy (hora local de los usuarios)
                    var targetLocal = new DateTime(localNow.Year, localNow.Month, localNow.Day, 20, 0, 0);
                    if (localNow >= targetLocal)
                    {
                        // Si ya pasaron las 20:00 hoy, programar para mañana a las 20:00
                        targetLocal = targetLocal.AddDays(1);
                    }

                    var targetUtc = TimeZoneInfo.ConvertTimeToUtc(targetLocal, _timeZone);
                    var delay = targetUtc - DateTimeOffset.UtcNow;

                    if (delay > TimeSpan.Zero)
                    {
                        _logger.LogInformation("Próxima alarma de coaches: {local} (esperando {h:F2} hrs)", targetLocal, delay.TotalHours);
                        await Task.Delay(delay, stoppingToken);
                    }

                    // Ejecutar la revisión de pendientes
                    await CheckAndSendRemindersAsync(stoppingToken);

                    // Pequeña espera para evitar que un bucle rápido re-dispare en el mismo segundo
                    await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en DailyCoachRoutineReminderWorker");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }

        private async Task CheckAndSendRemindersAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var notificationService = scope.ServiceProvider.GetRequiredService<IFirebaseNotificationService>();

            var nowUtc = DateTimeOffset.UtcNow;
            var localNow = TimeZoneInfo.ConvertTime(nowUtc, _timeZone);
            var tomorrow = localNow.Date.AddDays(1);
            _logger.LogInformation("Verificando rutinas pendientes para la fecha {date}", tomorrow);

            var coaches = await context.Coaches.AsNoTracking().ToListAsync(ct);

            foreach (var coach in coaches)
            {
                var studentIds = await context.CoachStudents.AsNoTracking()
                    .Where(cs => cs.CoachId == coach.Id && cs.Status)
                    .Select(cs => cs.StudentId)
                    .ToListAsync(ct);

                if (!studentIds.Any()) continue;

                var studentsWithRoutine = await context.DailyStudentExercises.AsNoTracking()
                    .Where(e => e.CoachId == coach.Id && e.ScheduledDate.Date == tomorrow && e.DeletedAt == null)
                    .Select(e => e.StudentId)
                    .Distinct()
                    .ToListAsync(ct);

                var pendingCount = studentIds.Count(id => !studentsWithRoutine.Contains(id));

                if (pendingCount > 0)
                {
                    _logger.LogInformation("Coach {coachId} (User {userId}) tiene {count} alumno(s) sin rutina para {tomorrow}. Enviando Push...", coach.Id, coach.UserId, pendingCount, tomorrow);
                    await notificationService.SendPushToUserAsync(
                        userId: coach.UserId,
                        title: "Rutinas pendientes para mañana 📋",
                        body: $"Tienes {pendingCount} alumno(s) sin rutina para mañana. ¡Planifícalas a tiempo!",
                        data: new Dictionary<string, string>
                        {
                            { "type", "coach_pending_routines" }
                        },
                        ct: ct
                    );
                }
            }
        }
    }
}
