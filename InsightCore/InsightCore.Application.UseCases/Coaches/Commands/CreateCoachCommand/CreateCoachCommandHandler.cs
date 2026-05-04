using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
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
            try
            {


                var coach = new Coach
                {
                    UserId = request.UserId,
                    Bio = request.Bio,
                    Certifications = request.Certifications,
                    IsVerified = false
                };

                var created = await _coachesRepository.InsertAsync(coach);
                var coachDto = _mapper.Map<CoachDto>(created);

                // 5. Retornar respuesta exitosa
                return new Response<CoachDto>
                {
                    Data = coachDto,
                    IsSuccess = true,
                    Message = "Coach registrado con éxito."
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
