using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Queries.GetDailyExercisesByStudentAndDateQuery
{
    public class GetDailyExercisesByStudentAndDateQuery : IRequest<Response<IEnumerable<AssignDailyExerciseDto>>>
    {
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
    }
}
