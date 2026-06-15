using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Queries.GetDailyExercisesByStudentAndDateStartAndEndQuery
{
    public class GetDailyExercisesByStudentAndDateStartAndEndQuery : IRequest<Response<IEnumerable<AssignDailyExerciseDto>>>
    {
        public int StudentId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
