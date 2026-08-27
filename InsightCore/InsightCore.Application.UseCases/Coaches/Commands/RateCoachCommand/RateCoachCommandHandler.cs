using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.Coaches.Commands.RateCoachCommand
{
    public class RateCoachCommandHandler : IRequestHandler<RateCoachCommand, Response<RateCoachDto>>
    {
        private readonly ICoachRatingsRepository _ratingsRepository;
        private readonly ICoachStudentsRepository _coachStudentsRepository;
        private readonly IMapper _mapper;

        public RateCoachCommandHandler(ICoachRatingsRepository ratingsRepository, ICoachStudentsRepository coachStudentsRepository, IMapper mapper)
        {
            _ratingsRepository = ratingsRepository;
            _coachStudentsRepository = coachStudentsRepository;
            _mapper = mapper;
        }

        public async Task<Response<RateCoachDto>> Handle(RateCoachCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<RateCoachDto>();

            // Verificar que el alumno esté asignado al coach y la relación esté activa
            var relation = await _coachStudentsRepository.GetByIdsAsync(request.CoachId, request.StudentId);
            if (relation == null || !relation.Status)
            {
                response.IsSuccess = false;
                response.Message = "El alumno no está asignado al coach o la relación no está activa.";
                return response;
            }

            // Preparar entidad para upsert
            var ratingEntity = new CoachRating
            {
                CoachId = request.CoachId,
                StudentId = request.StudentId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            var (persisted, wasUpdated) = await _ratingsRepository.UpsertAsync(ratingEntity);

            var dto = _mapper.Map<RateCoachDto>(persisted);
            dto.WasUpdated = wasUpdated;

            response.Data = dto;
            response.IsSuccess = true;
            response.Message = wasUpdated ? "Valoración actualizada." : "Valoración creada.";
            return response;
        }
    }
}
