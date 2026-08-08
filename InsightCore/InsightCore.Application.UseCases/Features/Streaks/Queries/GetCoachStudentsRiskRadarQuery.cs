using MediatR;
using PyrosFit.Application.DTOs;
using System.Collections.Generic;

namespace PyrosFit.Application.Features.Streaks.Queries
{
    public record GetCoachStudentsRiskRadarQuery(int CoachId) : IRequest<IEnumerable<CoachStudentRiskDto>>;
}
