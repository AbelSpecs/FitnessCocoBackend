using MediatR;
using System;

namespace PyrosFit.Application.Features.Streaks.Events
{
    public record DailyWorkoutCompletedNotification(int StudentId, DateTime ActivityDate) : INotification;
}
