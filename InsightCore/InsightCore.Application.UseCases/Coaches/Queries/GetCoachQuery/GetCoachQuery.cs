using InsightCore.Application.DTO;
using MediatR;

namespace InsightCore.Application.UseCases.Coaches.Queries.GetCoachQuery
{
    public class GetCoachQuery : IRequest<Response<CoachDto>>
    {
        public int Id { get; set; }
    }
}
