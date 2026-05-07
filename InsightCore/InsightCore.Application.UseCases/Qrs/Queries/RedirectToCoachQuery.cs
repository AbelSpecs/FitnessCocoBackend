using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Qrs.Queries
{
    public class RedirectToCoachQuery : IRequest<Response<QRTokenRegistroDto>>
    {
        public string Token { get; set; }
    }
}
