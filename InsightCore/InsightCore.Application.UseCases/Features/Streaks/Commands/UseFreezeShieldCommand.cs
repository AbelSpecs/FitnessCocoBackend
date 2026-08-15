using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;

namespace PyrosFit.Application.Features.Streaks.Commands
{
    public record UseFreezeShieldCommand(int StudentId, DateTime? ShieldDate = null) : IRequest<Response<StudentStreakDto>>;
}
