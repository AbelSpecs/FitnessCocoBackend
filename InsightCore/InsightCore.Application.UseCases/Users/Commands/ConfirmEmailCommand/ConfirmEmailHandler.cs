using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Text;

namespace InsightCore.Application.UseCases.Users.Commands.ConfirmEmailCommand
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmEmailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Decodificar el payload opaco Base64Url → "userId:token"
                int userId;
                string token;
                try
                {
                    var code = request.Code ?? string.Empty;

                    // Restaurar padding Base64 estándar
                    var base64 = code.Replace("-", "+").Replace("_", "/");
                    var pad = base64.Length % 4;
                    if (pad == 2) base64 += "==";
                    else if (pad == 3) base64 += "=";

                    var rawPayload = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

                    // Formato esperado: "{userId}:{token}"
                    var separatorIndex = rawPayload.IndexOf(':');
                    if (separatorIndex <= 0)
                        return new Response<string> { IsSuccess = false, Message = "El código de activación es inválido." };

                    userId = int.Parse(rawPayload[..separatorIndex]);
                    token = rawPayload[(separatorIndex + 1)..];
                }
                catch
                {
                    return new Response<string> { IsSuccess = false, Message = "El código de activación tiene un formato inválido." };
                }

                // 2. Buscar el usuario internamente usando el userId extraído del código opaco
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user is null)
                {
                    return new Response<string> { IsSuccess = false, Message = "El código de activación es inválido o ya expiró." };
                }

                if (user.EmailConfirmed)
                {
                    return new Response<string> { IsSuccess = true, Message = "El correo ya ha sido confirmado previamente." };
                }

                // 3. Comparar el token extraído del código con el almacenado en BD
                if (string.IsNullOrWhiteSpace(user.EmailConfirmationToken) || user.EmailConfirmationToken != token)
                {
                    return new Response<string> { IsSuccess = false, Message = "El código de activación es inválido o ya fue utilizado." };
                }

                if (!user.EmailConfirmationTokenExpiry.HasValue || user.EmailConfirmationTokenExpiry.Value <= DateTime.UtcNow)
                {
                    return new Response<string> { IsSuccess = false, Message = "El enlace de activación ha expirado. Solicita uno nuevo." };
                }

                // 4. Confirmar cuenta y limpiar token de BD
                user.EmailConfirmed = true;
                user.EmailConfirmationToken = null;
                user.EmailConfirmationTokenExpiry = null;

                var updated = await _unitOfWork.Users.UpdateAsync(user);

                if (!updated)
                {
                    return new Response<string> { IsSuccess = false, Message = "No se pudo actualizar el estado del usuario." };
                }

                return new Response<string> { IsSuccess = true, Message = "Correo confirmado. Ahora puedes iniciar sesión." };
            }
            catch (Exception ex)
            {
                return new Response<string> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        }
    }
}
