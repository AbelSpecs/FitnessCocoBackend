using InsightCore.Transversal.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace InsightCore.Application.UseCases.Users.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommand : IRequest<Response<string>>
    {
        /// <summary>
        /// Código opaco recibido en el enlace de recuperación (Base64Url de "userId:token") o token directo.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Nueva contraseña a establecer.
        /// </summary>
        [Required]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Confirmación de la nueva contraseña.
        /// </summary>
        public string? ConfirmPassword { get; set; }
    }
}
