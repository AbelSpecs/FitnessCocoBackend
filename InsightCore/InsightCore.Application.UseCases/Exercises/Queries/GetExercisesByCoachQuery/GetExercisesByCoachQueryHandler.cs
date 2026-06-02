using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByCoachQuery
{
    public class GetExercisesByCoachQueryHandler : IRequestHandler<GetExercisesByCoachQuery, Response<IEnumerable<ExerciseDto>>>
    {
        private readonly IExercisesRepository _repository;
        private readonly IMapper _mapper;

        public GetExercisesByCoachQueryHandler(IExercisesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ExerciseDto>>> Handle(GetExercisesByCoachQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<ExerciseDto>>();
            try
            {
                var list = await _repository.GetByCoachIdAsync(request.CoachId);
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
