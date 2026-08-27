using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InsightCore.Application.UseCases.Coaches.Queries.GetCoachProfileQuery
{
    public class GetCoachProfileQueryHandler : IRequestHandler<GetCoachProfileQuery, Response<CoachProfileDto>>
    {
        private readonly ICoachesRepository _coachesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ICoachRatingsRepository _ratingsRepository;
        private readonly IMapper _mapper;

        public GetCoachProfileQueryHandler(ICoachesRepository coachesRepository, IUsersRepository usersRepository, ICoachRatingsRepository ratingsRepository, IMapper mapper)
        {
            _coachesRepository = coachesRepository;
            _usersRepository = usersRepository;
            _ratingsRepository = ratingsRepository;
            _mapper = mapper;
        }

        public async Task<Response<CoachProfileDto>> Handle(GetCoachProfileQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<CoachProfileDto>();

            var coach = await _coachesRepository.GetByIdAsync(request.CoachId);
            if (coach == null)
            {
                response.IsSuccess = false;
                response.Message = "Coach no existe.";
                return response;
            }

            var user = await _usersRepository.GetByIdAsync(coach.UserId);
            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "Usuario del coach no encontrado.";
                return response;
            }

            var metrics = await _coachesRepository.GetCoachMetricsAsync(request.CoachId, request.ActiveThresholdDays);
            var (avg, totalCount) = await _ratingsRepository.GetCoachRatingStatsAsync(request.CoachId);

            var dto = new CoachProfileDto
            {
                Id = coach.Id,
                UserId = coach.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Bio = coach.Bio,
                Certifications = coach.Certifications,
                IsVerified = coach.IsVerified,
                YearsOfExperience = coach.YearsOfExperience,
                ProfilePicture = user.ProfilePicture,
                BannerPicture = user.BannerPicture,
                TotalStudents = metrics.TotalStudents,
                ActiveStudents = metrics.ActiveStudents,
                InactiveStudents = metrics.InactiveStudents,
                TotalRoutinesCreated = metrics.TotalRoutinesCreated,
                AverageRating = avg,
                TotalRatingsCount = totalCount
            };

            response.Data = dto;
            response.IsSuccess = true;
            response.Message = "Perfil coach obtenido.";
            return response;
        }
    }
}
