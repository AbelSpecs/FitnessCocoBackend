using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Coaches.Queries.GetStudentsListByCoachIdQuery
{
    public class GetStudentsListByCoachIdQuery : IRequest<Response<StudentListByCoachDto>>
    {
        public int CoachId { get; set; }
    }
}
