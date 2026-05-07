using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Coaches.Queries.GetCoachByUserIdQuery
{
    public class GetCoachByUserIdQuery : IRequest<Response<CoachDto>>
    {
        public int UserId { get; set; }
    }

}