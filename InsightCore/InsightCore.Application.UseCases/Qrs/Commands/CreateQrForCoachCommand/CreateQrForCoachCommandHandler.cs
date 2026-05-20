using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Application.UseCases.Students.Commands.CreateStudentCommand;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Qrs.Commands.CreateQrForCoachCommand
{
    public class CreateQrForCoachCommandHandler : IRequestHandler<CreateQrForCoachCommand, Response<QRTokenDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICoachesRepository _coachesRepository;
        private readonly IQrsRepository _qrsRepository;
        private readonly IQrService _qrService;

        public CreateQrForCoachCommandHandler(
            IUnitOfWork unitOfWork, IMapper mapper, ICoachesRepository coachesRepository, IQrsRepository qrsRepository,
            IQrService qrService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _coachesRepository = coachesRepository;
            _qrsRepository = qrsRepository;
            _qrService = qrService;
        }

        public async Task<Response<QRTokenDto>> Handle(CreateQrForCoachCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Buscar si ya existe un QR activo y que NO haya expirado
                var qrCoach = await _qrsRepository.GetByCoachIdAsync(request.CoachId);

                // Añadimos validación de tiempo para asegurar que realmente sigue vigente
                if (qrCoach != null && qrCoach.IsActive && qrCoach.ExpiresAt > DateTime.UtcNow)
                {
                    var dataQrExisting = BuildQrResponse(qrCoach.CoachId, qrCoach.Token, qrCoach.ExpiresAt);
                    return new Response<QRTokenDto> { IsSuccess = true, Data = dataQrExisting, Message = "Data QR found." };
                }

                // 2. Si no existe o expiró, validamos la existencia del Coach
                var coach = await _coachesRepository.GetByIdAsync(request.CoachId);
                if (coach == null)
                {
                    return new Response<QRTokenDto> { IsSuccess = false, Message = "Coach not found." };
                }

                // 3. Desactivar QRs anteriores de forma segura
                await _qrsRepository.DeactivateCoachTokensAsync(coach.Id);

                // 4. Crear nuevo token único
                var newToken = Guid.NewGuid().ToString("N"); 
                var qrToken = new CoachQRToken
                {
                    CoachId = coach.Id,
                    Token = newToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(1), // Se puede mover a un AppSettings/Config si varía
                    IsActive = true
                };

                await _qrsRepository.InsertAsync(qrToken);

                // 5. Generar respuesta unificada
                var dataQrNew = BuildQrResponse(qrToken.CoachId, qrToken.Token, qrToken.ExpiresAt);

                return new Response<QRTokenDto>
                {
                    IsSuccess = true,
                    Data = dataQrNew,
                    Message = "Generated data QR token created."
                };
            }
            catch (Exception ex)
            {
                // Recuerda usar ILogger aquí en producción para no exponer trazas de error crudas
                return new Response<QRTokenDto> { IsSuccess = false, Message = $"Error al procesar el código QR: {ex.Message}" };
            }
        }

        // 🛠️ Método privado de soporte para reutilizar la lógica de generación del QR y DTO
        private QRTokenDto BuildQrResponse(int coachId, string token, DateTime expiresAt)
        {
            // IMPORTANTE: Incluimos el token como QueryString para que el frontend de PyrosFit 
            // sepa de qué coach proviene el registro al escanearlo.
            string redirectUrl = $"https://pyrosfit.com/register-info?coachId=${coachId}";

            string base64 = _qrService.GenerateQrBase64(redirectUrl);

            return new QRTokenDto
            {
                CoachId = coachId,
                ExpiresAt = expiresAt,
                Base64 = base64
            };
        }
    }
}
