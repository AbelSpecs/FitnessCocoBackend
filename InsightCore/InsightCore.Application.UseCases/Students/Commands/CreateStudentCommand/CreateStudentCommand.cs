using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Students.Commands.CreateStudentCommand
{
    public class CreateStudentCommand : IRequest<Response<StudentDto>>
    {
        public CreateOrUpdateStudentDto Student { get; set; } = null!;
    }
}
