using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Queries.GetDailyExerciseSetQuery
{
    public class GetDailyExerciseSetQueryHandler : IRequestHandler<GetDailyExerciseSetQuery, Response<DailyExerciseSetDto>>
    {
        private readonly IDailyExerciseSetsRepository _repository;
        private readonly IMapper _mapper;

        public GetDailyExerciseSetQueryHandler(IDailyExerciseSetsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<DailyExerciseSetDto>> Handle(GetDailyExerciseSetQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<DailyExerciseSetDto>();
            try
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Set not found.";
                    return response;
                }
                response.Data = _mapper.Map<DailyExerciseSetDto>(entity);
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
