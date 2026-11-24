using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Commands.CreateExerciseCommand
{
    public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Response<ExerciseDto>>
    {
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IMuscleGroupsRepository _muscleGroupRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ICoachesRepository _coachesRepository;
        private readonly IMapper _mapper;

        public CreateExerciseCommandHandler(
            IExercisesRepository exercisesRepository,
            IMuscleGroupsRepository muscleGroupRepository,
            IUsersRepository usersRepository,
            ICoachesRepository coachesRepository,
            IMapper mapper)
        {
            _exercisesRepository = exercisesRepository;
            _muscleGroupRepository = muscleGroupRepository;
            _usersRepository = usersRepository;
            _coachesRepository = coachesRepository;
            _mapper = mapper;
        }

        public async Task<Response<ExerciseDto>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ExerciseDto>();
            try
            {
                var isGeneralExercise = request.Exercise.CoachId == null || request.Exercise.CoachId <= 0;

                if (isGeneralExercise)
                {
                    var requester = request.RequesterUserId.HasValue && request.RequesterUserId.Value > 0
                        ? await _usersRepository.GetByIdAsync(request.RequesterUserId.Value)
                        : null;

                    if (requester == null || !requester.IsAdmin)
                    {
                        response.IsSuccess = false;
                        response.Message = "No tienes permisos para crear ejercicios generales.";
                        return response;
                    }
                }
                else if (request.RequesterUserId.HasValue && request.RequesterUserId.Value > 0)
                {
                    var requester = await _usersRepository.GetByIdAsync(request.RequesterUserId.Value);
                    if (requester == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "No tienes permisos para crear este ejercicio.";
                        return response;
                    }

                    if (!requester.IsAdmin)
                    {
                        var coach = await _coachesRepository.GetByUserIdAsync(requester.Id);
                        if (coach == null || coach.Id != request.Exercise.CoachId)
                        {
                            response.IsSuccess = false;
                            response.Message = "No tienes permisos para crear ejercicios para otro entrenador.";
                            return response;
                        }
                    }
                }

                // validar que el MuscleGroup exista
                var mg = await _muscleGroupRepository.GetByIdAsync(request.Exercise.MuscleGroupId);
                if (mg == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Muscle group not found.";
                    return response;
                }

                var entity = _mapper.Map<Exercise>(request.Exercise);
                if (isGeneralExercise)
                {
                    entity.CoachId = null;
                    entity.IsCustom = false;
                }
                else
                {
                    entity.IsCustom = true;
                }

                var created = await _exercisesRepository.InsertAsync(entity);
                // cargar navegación para mapear nombre
                created = await _exercisesRepository.GetByIdAsync(created.Id);
                response.Data = _mapper.Map<ExerciseDto>(created);
                response.IsSuccess = true;
                response.Message = "Exercise created.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
