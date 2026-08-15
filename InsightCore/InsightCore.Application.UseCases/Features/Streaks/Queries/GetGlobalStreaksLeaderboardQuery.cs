using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;

namespace PyrosFit.Application.Features.Streaks.Queries
{
    public record GetGlobalStreaksLeaderboardQuery(int Limit = 20) : IRequest<Response<IEnumerable<StreakLeaderboardDto>>>;
}
