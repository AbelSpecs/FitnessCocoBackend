using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;

namespace PyrosFit.Application.Features.Streaks.Queries
{
    public record GetCoachStreaksLeaderboardQuery(int CoachId, int Limit = 10) : IRequest<Response<IEnumerable<StreakLeaderboardDto>>>;
}
