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
        private readonly IMapper _mapper;

        public CreateExerciseCommandHandler(IExercisesRepository exercisesRepository, IMapper mapper)
        {
            _exercisesRepository = exercisesRepository;
            _mapper = mapper;
        }

        public async Task<Response<ExerciseDto>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ExerciseDto>();
            try
            {
                var entity = _mapper.Map<Exercise>(request.Exercise);
                var created = await _exercisesRepository.InsertAsync(entity);
                response.Data = _mapper.Map<ExerciseDto>(created);
                response.IsSuccess = true;
                response.Message = "Exercise created successfully.";
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
