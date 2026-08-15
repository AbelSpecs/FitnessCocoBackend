using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;

namespace PyrosFit.Application.Features.Streaks.Queries
{
    public record GetStudentStreakLogsQuery(int StudentId, int Limit = 30) : IRequest<Response<IEnumerable<StreakLogDto>>>;
}
