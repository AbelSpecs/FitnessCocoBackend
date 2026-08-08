using MediatR;
using Microsoft.EntityFrameworkCore;
using PyrosFit.Application.Features.Streaks.Events;
using PyrosFit.Domain.Entities;
using InsightCore.Persistence.Contexts;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace PyrosFit.Application.Features.Streaks.Handlers
{
    public class UpdateStreakOnWorkoutCompletedHandler : INotificationHandler<DailyWorkoutCompletedNotification>
    {
        private readonly ApplicationDbContext _context;

        public UpdateStreakOnWorkoutCompletedHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DailyWorkoutCompletedNotification notification, CancellationToken cancellationToken)
        {
            var studentId = notification.StudentId;

            var streak = await _context.StudentStreaks.FindAsync(new object[] { studentId }, cancellationToken);
            var isNew = false;
            if (streak == null)
            {
                streak = new StudentStreak { StudentId = studentId };
                isNew = true;
            }

            var logs = streak.RecordActivity(notification.ActivityDate);

            if (isNew) _context.StudentStreaks.Add(streak);

            if (logs != null && logs.Any())
            {
                await _context.StreakLogs.AddRangeAsync(logs, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
