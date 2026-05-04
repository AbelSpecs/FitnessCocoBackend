using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Coaches.Queries.GetCoachQuery
{
    public class GetCoachQueryHandler : IRequestHandler<GetCoachQuery, Response<CoachDto>>
    {
        private readonly ICoachesRepository _coachesRepository;
        private readonly IMapper _mapper;

        public GetCoachQueryHandler(ICoachesRepository coachesRepository, IMapper mapper)
        {
            _coachesRepository = coachesRepository;
            _mapper = mapper;
        }

        public async Task<Response<CoachDto>> Handle(GetCoachQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response<CoachDto>();

                var coach = await _coachesRepository.GetByIdAsync(request.Id);

                if (coach is null)
                {
                    response.IsSuccess = true;
                    response.Message = "El Coach no existe.";
                    return response;
                }

                var coachDto = _mapper.Map<CoachDto>(coach);

                // Retornar respuesta exitosa
                return new Response<CoachDto>
                {
                    Data = coachDto,
                    IsSuccess = true,
                    Message = "Coach existente."
                };
            }
            catch (Exception ex)
            {
                // Loguear el error aquí
                return new Response<CoachDto> { IsSuccess = false, Message = $"Error: {ex.Message}" };
            }
        }
    }
}
