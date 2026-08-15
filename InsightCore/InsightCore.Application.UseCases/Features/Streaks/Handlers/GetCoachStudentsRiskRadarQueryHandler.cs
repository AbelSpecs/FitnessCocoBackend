using Dapper;
using InsightCore.Persistence.Contexts;
using InsightCore.Transversal.Common;
using MediatR;
using PyrosFit.Application.DTOs;
using PyrosFit.Application.Features.Streaks.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PyrosFit.Application.Features.Streaks.Handlers
{
    public class GetCoachStudentsRiskRadarQueryHandler : IRequestHandler<GetCoachStudentsRiskRadarQuery, Response<IEnumerable<CoachStudentRiskDto>>>
    {
        private readonly DapperContext _dapperContext;

        public GetCoachStudentsRiskRadarQueryHandler(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Response<IEnumerable<CoachStudentRiskDto>>> Handle(GetCoachStudentsRiskRadarQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<CoachStudentRiskDto>>();
            try
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
                response.Data = result.ToList();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
