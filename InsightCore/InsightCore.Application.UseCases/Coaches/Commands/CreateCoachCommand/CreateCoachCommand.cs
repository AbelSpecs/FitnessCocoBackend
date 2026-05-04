using InsightCore.Application.DTO;
using MediatR;

namespace InsightCore.Application.UseCases.Coaches.Commands.CreateCoachCommand
{
    public class CreateCoachCommand : IRequest<Response<CoachDto>>
    {
        public int UserId { get; set; }
        public string? Bio { get; set; }
        public string? Certifications { get; set; }
    }
}
