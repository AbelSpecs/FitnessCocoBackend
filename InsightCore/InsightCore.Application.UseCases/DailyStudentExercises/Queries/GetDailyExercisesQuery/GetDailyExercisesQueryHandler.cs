using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.UseCases.Exercises.Queries.GetExerciseQuery;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Queries.GetDailyExercisesQuery
{
    public class GetDailyExercisesQueryHandler : IRequestHandler<GetDailyExercisesQuery, Response<AssignDailyExerciseDto>>
    {
        private readonly IDailyStudentExercisesRepository _dailyRepo;
        private readonly IMapper _mapper;

        public GetDailyExercisesQueryHandler(IDailyStudentExercisesRepository dailyRepo, IMapper mapper)
        {
            _dailyRepo = dailyRepo;
            _mapper = mapper;
        }

        public async Task<Response<AssignDailyExerciseDto>> Handle(GetDailyExercisesQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<AssignDailyExerciseDto>();
            try
            {
                var entity = await _dailyRepo.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Daily exercise not found.";
                    return response;
                }
                response.Data = _mapper.Map<AssignDailyExerciseDto>(entity);
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
