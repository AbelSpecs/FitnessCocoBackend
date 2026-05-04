using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;

namespace InsightCore.Application.UseCases.Students.Queries.GetStudentsQuery
{
    public class GetStudentsQuery : IRequest<Response<IEnumerable<StudentDto>>>
    {
    }
}
