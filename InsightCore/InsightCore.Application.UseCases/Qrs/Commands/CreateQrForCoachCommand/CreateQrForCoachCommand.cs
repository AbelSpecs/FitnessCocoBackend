using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Qrs.Commands.CreateQrForCoachCommand
{
    public class CreateQrForCoachCommand : IRequest<Response<QRTokenDto>>
    {
        public int CoachId { get; set; }
    }
}
