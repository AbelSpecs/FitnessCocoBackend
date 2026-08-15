using Dapper;
using InsightCore.Application.DTO;
using InsightCore.Persistence.Contexts;
using InsightCore.Transversal.Common;
using MediatR;
using PyrosFit.Application.Features.Streaks.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PyrosFit.Application.Features.Streaks.Handlers
{
    public class GetCoachStreaksLeaderboardQueryHandler : IRequestHandler<GetCoachStreaksLeaderboardQuery, Response<IEnumerable<StreakLeaderboardDto>>>
    {
        private readonly DapperContext _dapperContext;

        public GetCoachStreaksLeaderboardQueryHandler(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Response<IEnumerable<StreakLeaderboardDto>>> Handle(GetCoachStreaksLeaderboardQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<StreakLeaderboardDto>>();
            try
            {
                using var conn = _dapperContext.CreateConnection();
                var sql = @"
                    SELECT 
                        ROW_NUMBER() OVER (ORDER BY COALESCE(ss.""CurrentStreak"", 0) DESC, COALESCE(ss.""LongestStreak"", 0) DESC, u.""FirstName"" ASC) AS ""Rank"",
                        s.""Id"" AS ""StudentId"",
                        (u.""FirstName"" || ' ' || u.""LastName"") AS ""StudentName"",
                        COALESCE(ss.""CurrentStreak"", 0) AS ""CurrentStreak"",
                        COALESCE(ss.""LongestStreak"", 0) AS ""LongestStreak"",
                        ss.""LastCompletedDate"",
                        COALESCE(ss.""FreezeShieldsAvailable"", 2) AS ""FreezeShieldsAvailable""
                    FROM ""CoachStudents"" cs
                    JOIN ""Students"" s ON cs.""StudentId"" = s.""Id""
                    JOIN ""Users"" u ON s.""UserId"" = u.""Id""
                    LEFT JOIN ""StudentStreaks"" ss ON ss.""StudentId"" = s.""Id""
                    WHERE cs.""CoachId"" = @CoachId AND cs.""Status"" = true
                    ORDER BY ""Rank"" ASC
                    LIMIT @Limit;";

                var list = await conn.QueryAsync<StreakLeaderboardDto>(sql, new { CoachId = request.CoachId, Limit = request.Limit });
                response.Data = list.ToList();
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
