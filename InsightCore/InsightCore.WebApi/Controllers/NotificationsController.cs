using System.Security.Claims;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsightCore.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class RegisterTokenDto
        {
            public string FcmToken { get; set; } = string.Empty;
            public string Platform { get; set; } = "android";
            public string? DeviceModel { get; set; }
        }

        public class RemoveTokenDto
        {
            public string FcmToken { get; set; } = string.Empty;
        }

        [HttpPost("register-token")]
        public async Task<IActionResult> RegisterToken([FromBody] RegisterTokenDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.FcmToken))
                return BadRequest("El token FCM es obligatorio.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || userId == 0)
                return Unauthorized();

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
            return Ok(new { success = true, message = "Token registrado exitosamente" });
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
    }
}
