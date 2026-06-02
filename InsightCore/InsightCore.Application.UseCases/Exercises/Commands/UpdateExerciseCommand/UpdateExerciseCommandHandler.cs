using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Commands.UpdateExerciseCommand
{
    public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, Response<ExerciseDto>>
    {
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IGenericRepository<MuscleGroup> _muscleGroupRepository;
        private readonly IMapper _mapper;

        public UpdateExerciseCommandHandler(IExercisesRepository exercisesRepository, IGenericRepository<MuscleGroup> muscleGroupRepository, IMapper mapper)
        {
            _exercisesRepository = exercisesRepository;
            _muscleGroupRepository = muscleGroupRepository;
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
                // validar muscle group
                var mg = await _muscleGroupRepository.GetAsync(request.Exercise.MuscleGroupId.ToString());
                if (mg == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Grupo muscular no encontrado.";
                    return response;
                }

                // map incoming dto to existing entity
                _mapper.Map(request.Exercise, existing);
                existing.Id = request.Id;
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
