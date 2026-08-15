using InsightCore.Transversal.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace InsightCore.Application.UseCases.Users.Commands.ConfirmEmailCommand
{
    public class ConfirmEmailCommand : IRequest<Response<string>>
    {
        /// <summary>
        /// Payload opaco Base64Url que contiene internamente el userId y el token de activación.
        /// El frontend lo lee del query param "code" del enlace de confirmación y lo envía aquí tal cual.
        /// </summary>
        [Required]
        public string? Code { get; set; }
    }
}
