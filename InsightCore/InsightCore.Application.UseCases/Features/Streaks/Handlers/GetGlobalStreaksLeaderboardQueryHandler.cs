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
    public class GetGlobalStreaksLeaderboardQueryHandler : IRequestHandler<GetGlobalStreaksLeaderboardQuery, Response<IEnumerable<StreakLeaderboardDto>>>
    {
        private readonly DapperContext _dapperContext;

        public GetGlobalStreaksLeaderboardQueryHandler(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Response<IEnumerable<StreakLeaderboardDto>>> Handle(GetGlobalStreaksLeaderboardQuery request, CancellationToken cancellationToken)
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
                    FROM ""Students"" s
                    JOIN ""Users"" u ON s.""UserId"" = u.""Id""
                    JOIN ""StudentStreaks"" ss ON ss.""StudentId"" = s.""Id""
                    WHERE ss.""CurrentStreak"" > 0
                    ORDER BY ""Rank"" ASC
                    LIMIT @Limit;";

                var list = await conn.QueryAsync<StreakLeaderboardDto>(sql, new { Limit = request.Limit });
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
