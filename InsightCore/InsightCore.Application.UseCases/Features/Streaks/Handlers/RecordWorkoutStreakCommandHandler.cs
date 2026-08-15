using InsightCore.Application.DTO;
using InsightCore.Persistence.Contexts;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PyrosFit.Application.Features.Streaks.Commands;
using PyrosFit.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PyrosFit.Application.Features.Streaks.Handlers
{
    public class RecordWorkoutStreakCommandHandler : IRequestHandler<RecordWorkoutStreakCommand, Response<StudentStreakDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public RecordWorkoutStreakCommandHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Response<StudentStreakDto>> Handle(RecordWorkoutStreakCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<StudentStreakDto>();
            try
            {
                var studentExists = await _context.Students.AnyAsync(s => s.Id == request.StudentId, cancellationToken);
                if (!studentExists)
                {
                    response.IsSuccess = false;
                    response.Message = "Student not found.";
                    return response;
                }

                var streak = await _context.StudentStreaks.FindAsync(new object[] { request.StudentId }, cancellationToken);
                var isNew = false;
                if (streak == null)
                {
                    streak = new StudentStreak { StudentId = request.StudentId };
                    isNew = true;
                }

                var logs = streak.RecordActivity(request.ActivityDate);

                if (isNew)
                {
                    _context.StudentStreaks.Add(streak);
                }

                if (logs != null && logs.Any())
                {
                    await _context.StreakLogs.AddRangeAsync(logs, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return await _mediator.Send(new Queries.GetStudentStreakQuery(request.StudentId), cancellationToken);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
