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
    public class GetStudentStreakLogsQueryHandler : IRequestHandler<GetStudentStreakLogsQuery, Response<IEnumerable<StreakLogDto>>>
    {
        private readonly DapperContext _dapperContext;

        public GetStudentStreakLogsQueryHandler(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Response<IEnumerable<StreakLogDto>>> Handle(GetStudentStreakLogsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<StreakLogDto>>();
            try
            {
                using var conn = _dapperContext.CreateConnection();
                var sql = @"
                    SELECT 
                        sl.""Id"",
                        sl.""StudentId"",
                        sl.""ActivityTypeId"",
                        COALESCE(sat.""Code"", '') AS ""ActivityTypeCode"",
                        COALESCE(sat.""Name"", 'Actividad') AS ""ActivityTypeName"",
                        sl.""ActivityDate"",
                        sl.""CreatedAt""
                    FROM ""StreakLogs"" sl
                    LEFT JOIN ""StreakActivityTypes"" sat ON sl.""ActivityTypeId"" = sat.""Id""
                    WHERE sl.""StudentId"" = @StudentId
                    ORDER BY sl.""ActivityDate"" DESC, sl.""CreatedAt"" DESC
                    LIMIT @Limit;";

                var logs = await conn.QueryAsync<StreakLogDto>(sql, new { StudentId = request.StudentId, Limit = request.Limit });
                response.Data = logs.ToList();
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
