using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;

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
                var response = new Response<string>();

                var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
                if (user is null)
                {
                    return new Response<string> { IsSuccess = false, Message = "Usuario no encontrado." };
                }

                if (user.EmailConfirmed)
                {
                    return new Response<string> { IsSuccess = true, Message = "El correo ya ha sido confirmado previamente." };
                }

                var incomingToken = string.Empty;
                try
                {
                    incomingToken = Uri.UnescapeDataString(request.Token ?? string.Empty);
                }
                catch
                {
                    incomingToken = request.Token ?? string.Empty;
                }

                if (string.IsNullOrWhiteSpace(user.EmailConfirmationToken) || user.EmailConfirmationToken != incomingToken)
                {
                    return new Response<string> { IsSuccess = false, Message = "El token de activación es inválido." };
                }

                if (!user.EmailConfirmationTokenExpiry.HasValue || user.EmailConfirmationTokenExpiry.Value <= DateTime.UtcNow)
                {
                    return new Response<string> { IsSuccess = false, Message = "El token de activación ha expirado." };
                }

                // Confirmar cuenta y limpiar tokens
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
