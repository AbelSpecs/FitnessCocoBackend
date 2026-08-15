using Dapper;
using InsightCore.Application.DTO;
using InsightCore.Persistence.Contexts;
using InsightCore.Transversal.Common;
using MediatR;
using PyrosFit.Application.Features.Streaks.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PyrosFit.Application.Features.Streaks.Handlers
{
    public class GetStudentStreakQueryHandler : IRequestHandler<GetStudentStreakQuery, Response<StudentStreakDto>>
    {
        private readonly DapperContext _dapperContext;

        public GetStudentStreakQueryHandler(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Response<StudentStreakDto>> Handle(GetStudentStreakQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<StudentStreakDto>();
            try
            {
                using var conn = _dapperContext.CreateConnection();
                var sql = @"
                    SELECT 
                        s.""Id"" AS ""StudentId"",
                        (u.""FirstName"" || ' ' || u.""LastName"") AS ""StudentName"",
                        COALESCE(ss.""CurrentStreak"", 0) AS ""CurrentStreak"",
                        COALESCE(ss.""LongestStreak"", 0) AS ""LongestStreak"",
                        ss.""LastCompletedDate"",
                        COALESCE(ss.""FreezeShieldsAvailable"", 2) AS ""FreezeShieldsAvailable"",
                        COALESCE(ss.""UpdatedAt"", CURRENT_TIMESTAMP) AS ""UpdatedAt"",
                        CASE 
                            WHEN ss.""LastCompletedDate"" = CURRENT_DATE THEN true 
                            ELSE false 
                        END AS ""IsCompletedToday"",
                        CASE 
                            WHEN ss.""LastCompletedDate"" IS NULL THEN 9999 
                            ELSE (CURRENT_DATE - ss.""LastCompletedDate"") 
                        END AS ""DaysInactive"",
                        CASE 
                            WHEN ss.""LastCompletedDate"" IS NULL THEN 'New'
                            WHEN (CURRENT_DATE - ss.""LastCompletedDate"") <= 1 THEN 'Active'
                            WHEN (CURRENT_DATE - ss.""LastCompletedDate"") = 2 AND COALESCE(ss.""FreezeShieldsAvailable"", 0) > 0 THEN 'Frozen'
                            WHEN (CURRENT_DATE - ss.""LastCompletedDate"") BETWEEN 2 AND 3 THEN 'AtRisk'
                            ELSE 'Broken'
                        END AS ""Status""
                    FROM ""Students"" s
                    JOIN ""Users"" u ON s.""UserId"" = u.""Id""
                    LEFT JOIN ""StudentStreaks"" ss ON ss.""StudentId"" = s.""Id""
                    WHERE s.""Id"" = @StudentId;";

                var streak = await conn.QueryFirstOrDefaultAsync<StudentStreakDto>(sql, new { StudentId = request.StudentId });
                if (streak == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Student not found.";
                    return response;
                }

                response.Data = streak;
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
