using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Students.Queries.GetStudentByUserIdQuery
{
    public class GetStudentByUserIdQuery : IRequest<Response<StudentDto>>
    {
        public int UserId { get; set; }
    }

}