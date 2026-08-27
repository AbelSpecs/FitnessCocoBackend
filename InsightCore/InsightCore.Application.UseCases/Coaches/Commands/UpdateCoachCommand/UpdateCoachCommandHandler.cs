using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Coaches.Commands.UpdateCoachCommand
{
    public class UpdateCoachCommandHandler : IRequestHandler<UpdateCoachCommand, Response<CoachDto>>
    {
        private readonly ICoachesRepository _coachesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UpdateCoachCommandHandler(ICoachesRepository coachesRepository, IUsersRepository usersRepository, IMapper mapper)
        {
            _coachesRepository = coachesRepository;
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<Response<CoachDto>> Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<CoachDto>();

            var coach = await _coachesRepository.GetByIdAsync(request.Id);
            if (coach == null)
            {
                response.IsSuccess = false;
                response.Message = "Coach no encontrado.";
                return response;
            }

            // Actualizar propiedades del coach
            coach.Bio = request.Bio ?? coach.Bio;
            coach.Certifications = request.Certifications ?? coach.Certifications;
            coach.YearsOfExperience = request.YearsOfExperience;

            var updated = await _coachesRepository.UpdateAsync(coach);
            if (!updated)
            {
                response.IsSuccess = false;
                response.Message = "No se pudo actualizar el coach.";
                return response;
            }

            // Si vienen imágenes, actualizar también la entidad User asociada
            if (!string.IsNullOrWhiteSpace(request.ProfilePicture) || !string.IsNullOrWhiteSpace(request.BannerPicture))
            {
                var user = await _usersRepository.GetByIdAsync(coach.UserId);
                if (user != null)
                {
                    if (!string.IsNullOrWhiteSpace(request.ProfilePicture)) user.ProfilePicture = request.ProfilePicture;
                    if (!string.IsNullOrWhiteSpace(request.BannerPicture)) user.BannerPicture = request.BannerPicture;
                    await _usersRepository.UpdateAsync(user);
                }
            }

            var dto = _mapper.Map<CoachDto>(coach);
            // rellenar imagenes desde el usuario si es posible
            var userAfter = await _usersRepository.GetByIdAsync(coach.UserId);
            if (userAfter != null)
            {
                dto.ProfilePicture = userAfter.ProfilePicture;
                dto.BannerPicture = userAfter.BannerPicture;
            }

            response.Data = dto;
            response.IsSuccess = true;
            response.Message = "Coach actualizado.";
            return response;
        }
    }
}
