using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByMuscleGroupQuery
{
    public class GetExercisesByMuscleGroupQueryHandler : IRequestHandler<GetExercisesByMuscleGroupQuery, Response<IEnumerable<ExerciseDto>>>
    {
        private readonly IExercisesRepository _repository;
        private readonly IMapper _mapper;

        public GetExercisesByMuscleGroupQueryHandler(IExercisesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ExerciseDto>>> Handle(GetExercisesByMuscleGroupQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<ExerciseDto>>();
            try
            {
                var list = await _repository.GetByMuscleGroupIdAsync(request.MuscleGroupId);
                response.Data = _mapper.Map<IEnumerable<ExerciseDto>>(list);
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
