using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;

namespace PyrosFit.Application.Features.Streaks.Commands
{
    public record RecordWorkoutStreakCommand(int StudentId, DateTime ActivityDate) : IRequest<Response<StudentStreakDto>>;
}
