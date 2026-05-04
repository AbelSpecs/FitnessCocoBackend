using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Students.Commands.DeleteStudentCommand
{
    public class DeleteStudentCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
    }
}
