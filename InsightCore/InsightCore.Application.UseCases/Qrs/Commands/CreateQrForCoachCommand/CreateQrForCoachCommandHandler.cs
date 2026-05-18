using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Application.UseCases.Students.Commands.CreateStudentCommand;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
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
                // 1. Obtener datos del coach
                var coach = await _coachesRepository.GetByIdAsync(request.CoachId);
                if (coach == null)
                {
                    return new Response<QRTokenDto> { IsSuccess = false, Message = "Coach not found." };
                }
                try
                {
                    // 2. Desactivar QRs anteriores (opcional, para que solo uno funcione a la vez)
                    await _qrsRepository.DeactivateCoachTokensAsync(coach.Id);

                }
                catch { }
                // 3. Crear nuevo token único
                var newToken = Guid.NewGuid().ToString("N"); // Token corto y limpio

                var qrToken = new CoachQRToken
                {
                    CoachId = coach.Id,
                    Token = newToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(1), // Duración del QR
                    IsActive = true
                };

                var created = await _qrsRepository.InsertAsync(qrToken);

                // Cambia esta URL por la de tu API real
                string redirectUrl = $"https://pyrosfit.com/api/v1/Qrs/redirect/{newToken}"; //cambiar por la URL real de tu API
                string base64 = _qrService.GenerateQrBase64(redirectUrl);
                var dataQr = new QRTokenDto();
                dataQr.ExpiresAt = qrToken.ExpiresAt;
                dataQr.CoachId = qrToken.CoachId;
                dataQr.Base64 = base64;

                return new Response<QRTokenDto> { IsSuccess = true, Data = dataQr, Message = "Generated data QR token created." };
            }
            catch (Exception ex)
            {
                return new Response<QRTokenDto> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
