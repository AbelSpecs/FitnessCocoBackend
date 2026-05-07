using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;

namespace InsightCore.Application.UseCases.Gyms.Queries.GetGymsQuery
{
    public class GetGymsQuery : IRequest<Response<IEnumerable<GymDto>>>
    {
    }
}
