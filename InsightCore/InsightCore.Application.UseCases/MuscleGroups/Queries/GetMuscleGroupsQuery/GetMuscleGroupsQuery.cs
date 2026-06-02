using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;

namespace InsightCore.Application.UseCases.MuscleGroups.Queries.GetMuscleGroupsQuery
{
    public class GetMuscleGroupsQuery : IRequest<Response<IEnumerable<MuscleGroupDto>>>
    {
    }
}
