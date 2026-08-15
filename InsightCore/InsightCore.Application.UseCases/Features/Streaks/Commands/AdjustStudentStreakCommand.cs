using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace PyrosFit.Application.Features.Streaks.Commands
{
    public record AdjustStudentStreakCommand(int StudentId, int? CurrentStreak, int? LongestStreak, int? FreezeShields, string? Reason = null) : IRequest<Response<StudentStreakDto>>;
}
