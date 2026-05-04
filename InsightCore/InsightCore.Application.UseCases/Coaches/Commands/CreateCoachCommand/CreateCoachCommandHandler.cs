using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using MediatR;

namespace InsightCore.Application.UseCases.Coaches.Commands.CreateCoachCommand
{
    public class CreateCoachCommandHandler : IRequestHandler<CreateCoachCommand, Response<CoachDto>>
    {
        private readonly ICoachesRepository _coachesRepository;
        private readonly IMapper _mapper;

        public CreateCoachCommandHandler(ICoachesRepository coachesRepository, IMapper mapper)
        {
            _coachesRepository = coachesRepository;
            _mapper = mapper;
        }

        public async Task<Response<CoachDto>> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
        {
            var coach = new Coach
            {
                UserId = request.UserId,
                Bio = request.Bio,
                Certifications = request.Certifications,
                IsVerified = false
            };

            var created = await _coachesRepository.InsertAsync(coach);
            var dto = _mapper.Map<CoachDto>(created);
            return new Response<CoachDto>(dto);
        }
    }
}
