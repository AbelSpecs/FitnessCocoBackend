using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Queries.GetDailyExercisesQuery
{
    public class GetDailyExercisesQuery : IRequest<Response<AssignDailyExerciseDto>>
    {
        public int Id { get; set; }
    }
}
