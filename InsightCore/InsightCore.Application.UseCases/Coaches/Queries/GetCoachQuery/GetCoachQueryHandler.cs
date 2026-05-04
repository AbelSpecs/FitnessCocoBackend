using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
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
            var coach = await _coachesRepository.GetByIdAsync(request.Id);
            if (coach == null) return new Response<CoachDto>("Coach not found");
            var dto = _mapper.Map<CoachDto>(coach);
            return new Response<CoachDto>(dto);
        }
    }
}
