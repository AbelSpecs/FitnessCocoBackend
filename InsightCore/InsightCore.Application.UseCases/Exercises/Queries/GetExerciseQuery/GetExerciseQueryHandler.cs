using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Queries.GetExerciseQuery
{
    public class GetExerciseQueryHandler : IRequestHandler<GetExerciseQuery, Response<ExerciseDto>>
    {
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IMapper _mapper;

        public GetExerciseQueryHandler(IExercisesRepository exercisesRepository, IMapper mapper)
        {
            _exercisesRepository = exercisesRepository;
            _mapper = mapper;
        }

        public async Task<Response<ExerciseDto>> Handle(GetExerciseQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<ExerciseDto>();
            try
            {
                var entity = await _exercisesRepository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Exercise not found.";
                    return response;
                }
                response.Data = _mapper.Map<ExerciseDto>(entity);
                response.IsSuccess = true;
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
