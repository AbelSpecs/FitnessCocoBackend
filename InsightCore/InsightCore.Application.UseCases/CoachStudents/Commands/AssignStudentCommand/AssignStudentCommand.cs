using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.CoachStudents.Commands.AssignStudentCommand
{
    public class AssignStudentCommand : IRequest<Response<CoachStudentDto>>
    {
        public int CoachId { get; set; }
        public int StudentId { get; set; }
        public bool Status { get; set; } = true;
    }
}
