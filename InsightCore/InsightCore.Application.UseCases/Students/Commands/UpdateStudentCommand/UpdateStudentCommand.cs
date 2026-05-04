using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Students.Commands.UpdateStudentCommand
{
    public class UpdateStudentCommand : IRequest<Response<StudentDto>>
    {
        public int Id { get; set; }
        public CreateOrUpdateStudentDto Student { get; set; } = null!;
    }
}
