using System.Security.Claims;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsightCore.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFirebaseNotificationService _notificationService;

        public NotificationsController(
            ApplicationDbContext context,
            IFirebaseNotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public class RegisterTokenDto
        {
            public int? UserId { get; set; }
            public string FcmToken { get; set; } = string.Empty;
            public string Platform { get; set; } = "android";
            public string? DeviceModel { get; set; }
        }

        public class RemoveTokenDto
        {
            public string FcmToken { get; set; } = string.Empty;
        }

        public class TestPushDto
        {
            public int UserId { get; set; }
            public string Title { get; set; } = "Notificación de prueba 🔔";
            public string Body { get; set; } = "Esta es una prueba del sistema de notificaciones de PyrosFit";
        }

        [HttpPost("register-token")]
        public async Task<IActionResult> RegisterToken([FromBody] RegisterTokenDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.FcmToken))
                return BadRequest("El token FCM es obligatorio.");

            int userId = dto.UserId ?? 0;
            if (userId == 0)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int.TryParse(userIdClaim, out userId);
            }

            if (userId == 0)
                return BadRequest("El UserId es obligatorio para vincular el token del dispositivo.");

            var existingToken = await _context.UserDeviceTokens
                .FirstOrDefaultAsync(t => t.FcmToken == dto.FcmToken, ct);

            if (existingToken != null)
            {
                existingToken.UserId = userId;
                existingToken.Platform = dto.Platform;
                existingToken.DeviceModel = dto.DeviceModel;
                existingToken.IsActive = true;
                existingToken.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                _context.UserDeviceTokens.Add(new UserDeviceToken
                {
                    UserId = userId,
                    FcmToken = dto.FcmToken,
                    Platform = dto.Platform,
                    DeviceModel = dto.DeviceModel,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync(ct);
            return Ok(new { success = true, message = $"Token registrado exitosamente para el usuario {userId}" });
        }

        [HttpDelete("remove-token")]
        public async Task<IActionResult> RemoveToken([FromBody] RemoveTokenDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.FcmToken))
                return BadRequest("El token es obligatorio.");

            var existingToken = await _context.UserDeviceTokens
                .FirstOrDefaultAsync(t => t.FcmToken == dto.FcmToken, ct);

            if (existingToken != null)
            {
                existingToken.IsActive = false;
                existingToken.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(ct);
            }

            return Ok(new { success = true, message = "Token desactivado" });
        }

        /// <summary>
        /// Obtiene los tokens registrados para un usuario (útil para auditoría y verificar si el móvil se registró)
        /// </summary>
        [HttpGet("user-tokens/{userId}")]
        public async Task<IActionResult> GetUserTokens(int userId, CancellationToken ct)
        {
            var tokens = await _context.UserDeviceTokens
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .ToListAsync(ct);

            return Ok(new { count = tokens.Count, tokens });
        }

        /// <summary>
        /// Endpoint para disparar un push de prueba directamente y verificar credenciales de Firebase
        /// </summary>
        [HttpPost("test-push")]
        public async Task<IActionResult> TestPush([FromBody] TestPushDto dto, CancellationToken ct)
        {
            if (dto.UserId <= 0)
                return BadRequest("Debe especificar un UserId válido.");

            var sent = await _notificationService.SendPushToUserAsync(
                dto.UserId,
                dto.Title,
                dto.Body,
                new Dictionary<string, string> { { "type", "test" } },
                ct);

            return Ok(new { success = sent, message = sent ? "Push enviado exitosamente al usuario" : "No se pudo enviar el push (verifique si el usuario tiene tokens activos)" });
        }

        /// <summary>
        /// Endpoint para disparar manualmente la lógica del Worker de las 20:00 (útil para pruebas y diagnósticos)
        /// </summary>
        [HttpPost("trigger-coach-reminders")]
        public async Task<IActionResult> TriggerCoachReminders(CancellationToken ct)
        {
            var tomorrow = DateTime.UtcNow.Date.AddDays(1);
            var coaches = await _context.Coaches.AsNoTracking().ToListAsync(ct);
            var results = new List<object>();

            foreach (var coach in coaches)
            {
                var studentIds = await _context.CoachStudents.AsNoTracking()
                    .Where(cs => cs.CoachId == coach.Id && cs.Status)
                    .Select(cs => cs.StudentId)
                    .ToListAsync(ct);

                if (!studentIds.Any()) continue;

                var studentsWithRoutine = await _context.DailyStudentExercises.AsNoTracking()
                    .Where(e => e.CoachId == coach.Id && e.ScheduledDate.Date == tomorrow && e.DeletedAt == null)
                    .Select(e => e.StudentId)
                    .Distinct()
                    .ToListAsync(ct);

                var pendingCount = studentIds.Count(id => !studentsWithRoutine.Contains(id));

                if (pendingCount > 0)
                {
                    var sent = await _notificationService.SendPushToUserAsync(
                        userId: coach.UserId,
                        title: "Rutinas pendientes para mañana 📋",
                        body: $"Tienes {pendingCount} alumno(s) sin rutina para mañana. ¡Planifícalas a tiempo!",
                        data: new Dictionary<string, string>
                        {
                            { "type", "coach_pending_routines" }
                        },
                        ct: ct
                    );

                    results.Add(new
                    {
                        coachId = coach.Id,
                        userId = coach.UserId,
                        pendingStudents = pendingCount,
                        notificationSent = sent
                    });
                }
            }

            return Ok(new
            {
                success = true,
                targetDate = tomorrow,
                coachesEvaluated = coaches.Count,
                results
            });
        }
    }
}
