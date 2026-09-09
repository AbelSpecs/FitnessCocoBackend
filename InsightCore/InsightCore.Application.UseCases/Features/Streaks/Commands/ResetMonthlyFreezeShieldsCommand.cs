using MediatR;

namespace PyrosFit.Application.Features.Streaks.Commands
{
    public record ResetMonthlyFreezeShieldsCommand(string Period) : IRequest<Unit>;
}
