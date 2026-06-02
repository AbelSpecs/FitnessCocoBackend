using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Queries.GetDailyExerciseSetsQuery
{
    public class GetDailyExerciseSetsQueryHandler : IRequestHandler<GetDailyExerciseSetsQuery, Response<IEnumerable<DailyExerciseSetDto>>>
    {
        private readonly IDailyExerciseSetsRepository _repository;
        private readonly IMapper _mapper;

        public GetDailyExerciseSetsQueryHandler(IDailyExerciseSetsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<DailyExerciseSetDto>>> Handle(GetDailyExerciseSetsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<DailyExerciseSetDto>>();
            try
            {
                var entities = await _repository.GetAllAsync();
                if (entities == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No sets found.";
                    return response;
                }

                response.Data = _mapper.Map<IEnumerable<DailyExerciseSetDto>>(entities.ToList());
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
