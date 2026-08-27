using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.Students.Commands.SendMotivationEmailCommand
{
    public class SendMotivationEmailHandler : IRequestHandler<SendMotivationEmailCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogger<SendMotivationEmailHandler> _logger;

        public SendMotivationEmailHandler(
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            ILogger<SendMotivationEmailHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(SendMotivationEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string targetEmail = string.Empty;
                string studentDisplayName = "Alumno";

                // 1. Buscar alumno por StudentId
                if (request.StudentId > 0)
                {
                    var student = await _unitOfWork.Students.GetByIdAsync(request.StudentId);
                    if (student is not null)
                    {
                        var user = await _unitOfWork.Users.GetByIdAsync(student.UserId);
                        if (user is not null)
                        {
                            targetEmail = user.Email;
                            studentDisplayName = $"{user.FirstName} {user.LastName}".Trim();
                        }
                    }
                }

                // 2. Si no se encontró por StudentId o se envió correo directo
                if (string.IsNullOrWhiteSpace(targetEmail) && !string.IsNullOrWhiteSpace(request.StudentEmail))
                {
                    targetEmail = request.StudentEmail.Trim();
                    var user = await _unitOfWork.Users.GetByEmailAsync(targetEmail);
                    if (user is not null)
                    {
                        studentDisplayName = $"{user.FirstName} {user.LastName}".Trim();
                    }
                }

                if (string.IsNullOrWhiteSpace(targetEmail))
                {
                    return new Response<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "No se encontró la dirección de correo electrónico del alumno."
                    };
                }

                // 3. Enviar correo usando el servicio integrado (Resend)
                await _emailService.SendMotivationEmailAsync(
                    targetEmail,
                    studentDisplayName,
                    request.Message,
                    request.CoachName);

                return new Response<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "Correo motivacional enviado exitosamente al alumno."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar correo motivacional para StudentId {StudentId}", request.StudentId);
                return new Response<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = $"Error al enviar el correo motivacional: {ex.Message}"
                };
            }
        }
    }
}
