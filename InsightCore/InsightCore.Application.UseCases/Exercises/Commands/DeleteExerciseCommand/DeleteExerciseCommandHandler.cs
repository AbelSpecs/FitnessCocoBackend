using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Commands.DeleteExerciseCommand
{
    public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, Response<bool>>
    {
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ICoachesRepository _coachesRepository;

        public DeleteExerciseCommandHandler(
            IExercisesRepository exercisesRepository,
            IUsersRepository usersRepository,
            ICoachesRepository coachesRepository)
        {
            _exercisesRepository = exercisesRepository;
            _usersRepository = usersRepository;
            _coachesRepository = coachesRepository;
        }

        public async Task<Response<bool>> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                var existing = await _exercisesRepository.GetByIdAsync(request.Id);
                if (existing == null)
                {
                    response.IsSuccess = false;
                    response.Data = false;
                    response.Message = "Exercise not found.";
                    return response;
                }

                if (existing.CoachId == null)
                {
                    var requester = request.RequesterUserId.HasValue && request.RequesterUserId.Value > 0
                        ? await _usersRepository.GetByIdAsync(request.RequesterUserId.Value)
                        : null;

                    if (requester == null || !requester.IsAdmin)
                    {
                        response.IsSuccess = false;
                        response.Data = false;
                        response.Message = "No tienes permisos para eliminar ejercicios generales.";
                        return response;
                    }
                }
                else if (request.RequesterUserId.HasValue && request.RequesterUserId.Value > 0)
                {
                    var requester = await _usersRepository.GetByIdAsync(request.RequesterUserId.Value);
                    if (requester == null)
                    {
                        response.IsSuccess = false;
                        response.Data = false;
                        response.Message = "No tienes permisos para eliminar este ejercicio.";
                        return response;
                    }

                    if (!requester.IsAdmin)
                    {
                        var coach = await _coachesRepository.GetByUserIdAsync(requester.Id);
                        if (coach == null || coach.Id != existing.CoachId)
                        {
                            response.IsSuccess = false;
                            response.Data = false;
                            response.Message = "No tienes permisos para eliminar este ejercicio.";
                            return response;
                        }
                    }
                }

                var deleted = await _exercisesRepository.DeleteAsync(request.Id);
                response.IsSuccess = deleted;
                response.Data = deleted;
                response.Message = deleted ? "Exercise deleted." : "Exercise not found.";
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
