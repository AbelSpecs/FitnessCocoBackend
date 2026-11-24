using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Commands.UpdateExerciseCommand
{
    public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, Response<ExerciseDto>>
    {
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IMuscleGroupsRepository _muscleGroupRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ICoachesRepository _coachesRepository;
        private readonly IMapper _mapper;

        public UpdateExerciseCommandHandler(
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

        public async Task<Response<ExerciseDto>> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ExerciseDto>();
            try
            {
                var existing = await _exercisesRepository.GetByIdAsync(request.Id);
                if (existing == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Ejercicio no encontrado.";
                    return response;
                }

                var isGeneralExercise = existing.CoachId == null || !existing.IsCustom;
                if (isGeneralExercise)
                {
                    var requester = request.RequesterUserId.HasValue && request.RequesterUserId.Value > 0
                        ? await _usersRepository.GetByIdAsync(request.RequesterUserId.Value)
                        : null;

                    if (requester == null || !requester.IsAdmin)
                    {
                        response.IsSuccess = false;
                        response.Message = "No tienes permisos para editar ejercicios generales.";
                        return response;
                    }
                }
                else if (request.RequesterUserId.HasValue && request.RequesterUserId.Value > 0)
                {
                    var requester = await _usersRepository.GetByIdAsync(request.RequesterUserId.Value);
                    if (requester == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "No tienes permisos para editar este ejercicio.";
                        return response;
                    }

                    if (!requester.IsAdmin)
                    {
                        var coach = await _coachesRepository.GetByUserIdAsync(requester.Id);
                        if (coach == null || coach.Id != existing.CoachId)
                        {
                            response.IsSuccess = false;
                            response.Message = "No tienes permisos para editar este ejercicio.";
                            return response;
                        }
                    }
                }

                // validar muscle group
                var mg = await _muscleGroupRepository.GetByIdAsync(request.Exercise.MuscleGroupId);
                if (mg == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Muscle group not found.";
                    return response;
                }

                // Preservar propiedad de ejercicio general vs personalizado
                var originalCoachId = existing.CoachId;
                var originalIsCustom = existing.IsCustom;

                // map incoming dto to existing entity
                _mapper.Map(request.Exercise, existing);
                existing.Id = request.Id;
                existing.CoachId = originalCoachId;
                existing.IsCustom = originalIsCustom;

                var updated = await _exercisesRepository.UpdateAsync(existing);
                response.IsSuccess = updated;
                response.Data = _mapper.Map<ExerciseDto>(existing);
                response.Message = updated ? "Ejercicio actualizado." : "Sin cambios.";
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
