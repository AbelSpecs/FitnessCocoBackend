using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Queries.GetDailyExerciseSetsQuery
{
    public class GetDailyExerciseSetsQuery : IRequest<Response<IEnumerable<DailyExerciseSetDto>>>
    {
    }
}
