using InsightCore.Transversal.Common;
using MediatR;
using InsightCore.Application.DTO;

namespace InsightCore.Application.UseCases.Coaches.Queries.GetCoachProfileQuery
{
    public class GetCoachProfileQuery : IRequest<Response<CoachProfileDto>>
    {
        public int CoachId { get; set; }
        public int ActiveThresholdDays { get; set; } = 30;
    }
}
