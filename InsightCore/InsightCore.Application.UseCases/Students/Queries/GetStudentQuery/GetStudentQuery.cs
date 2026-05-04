using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Students.Queries.GetStudentQuery
{
    public class GetStudentQuery : IRequest<Response<StudentDto>>
    {
        public int Id { get; set; }
    }
}
