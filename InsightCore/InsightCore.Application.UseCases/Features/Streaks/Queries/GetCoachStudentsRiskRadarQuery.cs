using InsightCore.Transversal.Common;
using MediatR;
using PyrosFit.Application.DTOs;
using System.Collections.Generic;

namespace PyrosFit.Application.Features.Streaks.Queries
{
    public record GetCoachStudentsRiskRadarQuery(int CoachId) : IRequest<Response<IEnumerable<CoachStudentRiskDto>>>;
}
