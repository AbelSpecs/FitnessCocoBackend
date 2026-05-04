using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Coaches.Queries.GetCoachQuery
{
    public class GetCoachQuery : IRequest<Response<CoachDto>>
    {
        public int Id { get; set; }
    }
}
