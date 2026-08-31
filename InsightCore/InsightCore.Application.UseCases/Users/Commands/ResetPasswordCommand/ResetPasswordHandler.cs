using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.Users.Commands.ResetPasswordCommand
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogger<ResetPasswordHandler> _logger;

        public ResetPasswordHandler(
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            ILogger<ResetPasswordHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                int? userId = null;
                string token = request.Code.Trim();

                // 1. Intentar decodificar el payload Base64Url opaco ("userId:token")
                try
                {
                    var base64 = token.Replace("-", "+").Replace("_", "/");
                    var pad = base64.Length % 4;
                    if (pad == 2) base64 += "==";
                    else if (pad == 3) base64 += "=";

                    var rawPayload = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                    var separatorIndex = rawPayload.IndexOf(':');
                    if (separatorIndex > 0 && int.TryParse(rawPayload[..separatorIndex], out var parsedId))
                    {
                        userId = parsedId;
                        token = rawPayload[(separatorIndex + 1)..];
                    }
                }
                catch
                {
                    // Si no es un payload Base64Url con formato "userId:token", se utiliza el token tal cual
                }

                User? user = null;
                if (userId.HasValue)
                {
                    user = await _unitOfWork.Users.GetByIdAsync(userId.Value);
                }

                if (user is null)
                {
                    return new Response<string>
                    {
                        IsSuccess = false,
                        Message = "El código o enlace de recuperación es inválido o ha expirado."
                    };
                }

                // 2. Validar token y expiración
                if (string.IsNullOrWhiteSpace(user.PasswordResetToken) || user.PasswordResetToken != token)
                {
                    return new Response<string>
                    {
                        IsSuccess = false,
                        Message = "El token de recuperación es inválido o ya ha sido utilizado."
                    };
                }

                if (!user.PasswordResetTokenExpiry.HasValue || user.PasswordResetTokenExpiry.Value <= DateTime.UtcNow)
                {
                    return new Response<string>
                    {
                        IsSuccess = false,
                        Message = "El enlace de recuperación ha expirado. Por favor, solicita uno nuevo."
                    };
                }

                // 3. Actualizar contraseña y limpiar token
                user.SetSecurePassword(request.NewPassword);
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpiry = null;
                user.AccessFailedCount = 0;
                user.ResetAccessFailedCount();

                var updated = await _unitOfWork.Users.UpdateAsync(user);
                if (!updated)
                {
                    return new Response<string>
                    {
                        IsSuccess = false,
                        Message = "No fue posible actualizar la contraseña. Intenta nuevamente."
                    };
                }

                // 4. Notificar al usuario por correo electrónico del cambio exitoso por motivos de seguridad
                try
                {
                    await _emailService.SendPasswordChangedNotificationEmailAsync(user.Email, user.FirstName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al enviar correo de notificación de contraseña actualizada a {Email}", user.Email);
                }

                return new Response<string>
                {
                    IsSuccess = true,
                    Message = "Contraseña restablecida exitosamente. Ya puedes iniciar sesión con tu nueva clave."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al restablecer la contraseña");
                return new Response<string>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
