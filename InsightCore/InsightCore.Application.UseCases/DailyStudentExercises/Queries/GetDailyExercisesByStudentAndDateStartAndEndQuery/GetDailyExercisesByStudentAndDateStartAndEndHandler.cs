using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Queries.GetDailyExercisesByStudentAndDateStartAndEndQuery
{
    public class GetDailyExercisesByStudentAndDateStartAndEndHandler : IRequestHandler<GetDailyExercisesByStudentAndDateStartAndEndQuery, Response<IEnumerable<AssignDailyExerciseDto>>> 
    {
        private readonly IDailyStudentExercisesRepository _dailyRepo;
        private readonly IMapper _mapper;

        public GetDailyExercisesByStudentAndDateStartAndEndHandler(IDailyStudentExercisesRepository dailyRepo, IMapper mapper)
        {
            _dailyRepo = dailyRepo;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<AssignDailyExerciseDto>>> Handle(GetDailyExercisesByStudentAndDateStartAndEndQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<AssignDailyExerciseDto>>();
            try
            {
                var list = await _dailyRepo.GetByStudentAndDateStartAndEndAsync(request.StudentId, request.DateStart, request.DateEnd);
                // Ensure sets ordered by SetNumber
                foreach (var item in list)
                {
                    item.DailyExerciseSets = item.DailyExerciseSets.OrderBy(s => s.SetNumber).ToList();
                }

                var listItems = list.ToList();
                var dtos = _mapper.Map<List<AssignDailyExerciseDto>>(listItems);
                for (int i = 0; i < listItems.Count; i++)
                {
                    dtos[i].ExerciseName = listItems[i].Exercise?.Name;
                    dtos[i].MuscleGroupName = listItems[i].Exercise?.MuscleGroup?.Name;
                }
                response.Data = dtos;
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
