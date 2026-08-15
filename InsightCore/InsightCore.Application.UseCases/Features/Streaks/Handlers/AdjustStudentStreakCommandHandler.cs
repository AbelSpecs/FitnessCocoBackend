using InsightCore.Application.DTO;
using InsightCore.Persistence.Contexts;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PyrosFit.Application.Features.Streaks.Commands;
using PyrosFit.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PyrosFit.Application.Features.Streaks.Handlers
{
    public class AdjustStudentStreakCommandHandler : IRequestHandler<AdjustStudentStreakCommand, Response<StudentStreakDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public AdjustStudentStreakCommandHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Response<StudentStreakDto>> Handle(AdjustStudentStreakCommand request, CancellationToken cancellationToken)
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
                if (streak == null)
                {
                    streak = new StudentStreak { StudentId = request.StudentId };
                    _context.StudentStreaks.Add(streak);
                }

                var log = streak.AdjustStreak(request.CurrentStreak, request.LongestStreak, request.FreezeShields);
                if (log != null)
                {
                    await _context.StreakLogs.AddAsync(log, cancellationToken);
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
