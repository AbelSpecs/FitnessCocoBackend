using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Coaches.Commands.RateCoachCommand
{
    public class RateCoachCommand : IRequest<Response<InsightCore.Application.DTO.RateCoachDto>>
    {
        public int CoachId { get; set; }
        public int StudentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
