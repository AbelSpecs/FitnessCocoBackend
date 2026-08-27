using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.Users.Commands.ForgotPasswordCommand
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ForgotPasswordHandler> _logger;

        public ForgotPasswordHandler(
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IConfiguration configuration,
            ILogger<ForgotPasswordHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            const string genericSuccessMessage = "Si el correo electrónico se encuentra registrado, recibirás un enlace para restablecer tu contraseña.";

            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(request.Email.Trim().ToLower());

                // Por seguridad y prevención de enumeración de cuentas, respondemos éxito genérico si el usuario no existe
                if (user is null)
                {
                    return new Response<string>
                    {
                        IsSuccess = true,
                        Message = genericSuccessMessage
                    };
                }

                // Generar token criptográfico seguro
                var tokenBytes = RandomNumberGenerator.GetBytes(32);
                var resetToken = Convert.ToBase64String(tokenBytes)
                    .Replace("+", "-").Replace("/", "_").TrimEnd('=');

                // Asignar token y expiración de 2 horas
                user.PasswordResetToken = resetToken;
                user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(2);

                var updated = await _unitOfWork.Users.UpdateAsync(user);
                if (!updated)
                {
                    return new Response<string>
                    {
                        IsSuccess = false,
                        Message = "No fue posible procesar la solicitud de recuperación en este momento."
                    };
                }

                // Generar enlace seguro opaco (Base64Url con userId:token)
                var frontendUrl = _configuration["AppSettings:FrontendUrl"]?.TrimEnd('/') ?? "https://pyrosfit.com";
                var rawPayload = $"{user.Id}:{resetToken}";
                var opaqueCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawPayload))
                    .Replace("+", "-").Replace("/", "_").TrimEnd('=');

                var resetLink = $"{frontendUrl}/reset-password?code={opaqueCode}";

                try
                {
                    await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink, user.FirstName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al enviar el correo de restablecimiento de contraseña a {Email}", user.Email);
                }

                return new Response<string>
                {
                    IsSuccess = true,
                    Message = genericSuccessMessage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado en ForgotPasswordHandler para email {Email}", request.Email);
                return new Response<string>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
