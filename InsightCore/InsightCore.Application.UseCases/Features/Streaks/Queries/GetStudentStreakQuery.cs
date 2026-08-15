using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace PyrosFit.Application.Features.Streaks.Queries
{
    public record GetStudentStreakQuery(int StudentId) : IRequest<Response<StudentStreakDto>>;
}
