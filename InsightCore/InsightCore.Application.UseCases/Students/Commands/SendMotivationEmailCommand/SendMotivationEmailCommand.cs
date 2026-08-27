using InsightCore.Transversal.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace InsightCore.Application.UseCases.Students.Commands.SendMotivationEmailCommand
{
    public class SendMotivationEmailCommand : IRequest<Response<bool>>
    {
        /// <summary>
        /// Identificador del alumno.
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Correo electrónico directo del alumno (opcional si se provee StudentId).
        /// </summary>
        [EmailAddress]
        public string? StudentEmail { get; set; }

        /// <summary>
        /// Mensaje motivacional personalizado redactado o seleccionado por el entrenador.
        /// </summary>
        [Required]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del entrenador que envía la motivación (opcional).
        /// </summary>
        public string? CoachName { get; set; }
    }
}
