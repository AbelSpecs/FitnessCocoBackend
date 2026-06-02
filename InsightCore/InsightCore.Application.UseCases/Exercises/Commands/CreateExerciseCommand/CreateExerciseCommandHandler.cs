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
        private readonly IGenericRepository<MuscleGroup> _muscleGroupRepository;
        private readonly IMapper _mapper;

        public CreateExerciseCommandHandler(IExercisesRepository exercisesRepository, IGenericRepository<MuscleGroup> muscleGroupRepository, IMapper mapper)
        {
            _exercisesRepository = exercisesRepository;
            _muscleGroupRepository = muscleGroupRepository;
            _mapper = mapper;
        }

        public async Task<Response<ExerciseDto>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ExerciseDto>();
            try
            {
                // validar que el MuscleGroup exista
                var mg = await _muscleGroupRepository.GetAsync(request.Exercise.MuscleGroupId.ToString());
                if (mg == null)
                {
                    response.IsSuccess = false;
                    return response;
                }

                var entity = _mapper.Map<Exercise>(request.Exercise);
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
