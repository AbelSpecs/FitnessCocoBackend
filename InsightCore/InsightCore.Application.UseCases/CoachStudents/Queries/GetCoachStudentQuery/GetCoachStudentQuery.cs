using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.CoachStudents.Queries.GetCoachStudentQuery
{
    public class GetCoachStudentQuery : IRequest<Response<CoachStudentDto>>
    {
        public int CoachId { get; set; }
        public int StudentId { get; set; }
    }
}
