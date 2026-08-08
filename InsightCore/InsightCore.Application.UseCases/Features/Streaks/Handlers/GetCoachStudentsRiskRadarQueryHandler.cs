using Dapper;
using MediatR;
using PyrosFit.Application.DTOs;
using InsightCore.Persistence.Contexts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace PyrosFit.Application.Features.Streaks.Handlers
{
    public class GetCoachStudentsRiskRadarQueryHandler : IRequestHandler<PyrosFit.Application.Features.Streaks.Queries.GetCoachStudentsRiskRadarQuery, IEnumerable<CoachStudentRiskDto>>
    {
        private readonly DapperContext _dapperContext;

        public GetCoachStudentsRiskRadarQueryHandler(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<CoachStudentRiskDto>> Handle(PyrosFit.Application.Features.Streaks.Queries.GetCoachStudentsRiskRadarQuery request, CancellationToken cancellationToken)
        {
            using var conn = _dapperContext.CreateConnection();

            var sql = @"
                SELECT s.""Id"" AS ""StudentId"",
                       (u.""FirstName"" || ' ' || u.""LastName"") AS ""StudentName"",
                       COALESCE(ss.""CurrentStreak"", 0) AS ""CurrentStreak"",
                       CASE WHEN ss.""LastCompletedDate"" IS NULL THEN 9999 ELSE (CURRENT_DATE - ss.""LastCompletedDate"") END AS ""DaysInactive"",
                       CASE WHEN ss.""LastCompletedDate"" IS NULL THEN 2 WHEN (CURRENT_DATE - ss.""LastCompletedDate"") <= 1 THEN 0 WHEN (CURRENT_DATE - ss.""LastCompletedDate"") BETWEEN 2 AND 3 THEN 1 ELSE 2 END AS ""RiskLevel""
                FROM ""CoachStudents"" cs
                JOIN ""Students"" s ON cs.""StudentId"" = s.""Id""
                JOIN ""Users"" u ON s.""UserId"" = u.""Id""
                LEFT JOIN ""StudentStreaks"" ss ON ss.""StudentId"" = s.""Id""
                WHERE cs.""CoachId"" = @CoachId AND cs.""Status"" = true
                ORDER BY ""RiskLevel"" DESC;";

            var result = await conn.QueryAsync<CoachStudentRiskDto>(sql, new { CoachId = request.CoachId });
            return result.ToList();
        }
    }
}
