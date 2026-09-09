using InsightCore.Persistence.Contexts;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PyrosFit.Application.Features.Streaks.Commands;
using PyrosFit.Domain.Entities;
using System;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace PyrosFit.Application.Features.Streaks.Handlers
{
    public class ResetMonthlyFreezeShieldsCommandHandler : IRequestHandler<ResetMonthlyFreezeShieldsCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ResetMonthlyFreezeShieldsCommandHandler> _logger;

        public ResetMonthlyFreezeShieldsCommandHandler(ApplicationDbContext context, ILogger<ResetMonthlyFreezeShieldsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(ResetMonthlyFreezeShieldsCommand request, CancellationToken cancellationToken)
        {
            var period = request.Period;
            _logger.LogInformation("Starting ResetMonthlyFreezeShields for period {period}", period);

            const int maxRetries = 3;
            var attempt = 0;

            while (true)
            {
                attempt++;
                using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
                try
                {
                    var already = await _context.SystemJobLogs
                        .AsNoTracking()
                        .AnyAsync(x => x.Period == period && x.JobName == "ResetMonthlyFreezeShields", cancellationToken);

                    if (already)
                    {
                        _logger.LogInformation("Period {period} already processed. Nothing to do.", period);
                        await tx.RollbackAsync(cancellationToken);
                        return Unit.Value;
                    }

                    // Update in batch: set FreezeShieldsAvailable = 2 for those with < 2
                    var affected = await _context.StudentStreaks
                        .Where(s => s.FreezeShieldsAvailable < 2)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.FreezeShieldsAvailable, p => 2), cancellationToken);

                    var log = new InsightCore.Domain.Entities.SystemJobLog
                    {
                        Period = period,
                        JobName = "ResetMonthlyFreezeShields",
                        CreatedAt = DateTimeOffset.UtcNow,
                        CompletedAt = DateTimeOffset.UtcNow,
                        Notes = $"RowsUpdated={affected}"
                    };

                    _context.SystemJobLogs.Add(log);
                    await _context.SaveChangesAsync(cancellationToken);

                    await tx.CommitAsync(cancellationToken);

                    _logger.LogInformation("ResetMonthlyFreezeShields completed for {period}. Rows affected: {count}", period, affected);
                    return Unit.Value;
                }
                catch (DbUpdateException dbEx) when (IsSerializationFailure(dbEx))
                {
                    await tx.RollbackAsync(cancellationToken);
                    if (attempt >= maxRetries)
                    {
                        _logger.LogError(dbEx, "Serialization failure, attempts exhausted for period {period}", period);
                        throw;
                    }
                    _logger.LogWarning(dbEx, "Serialization conflict attempt {attempt} for period {period}, retrying...", attempt, period);
                    await Task.Delay(TimeSpan.FromSeconds(2 * attempt), cancellationToken);
                    continue;
                }
                catch (PostgresException pex) when (pex.SqlState == "40001")
                {
                    await tx.RollbackAsync(cancellationToken);
                    if (attempt >= maxRetries)
                    {
                        _logger.LogError(pex, "Postgres serialization failure, attempts exhausted for period {period}", period);
                        throw;
                    }
                    _logger.LogWarning(pex, "Postgres serialization conflict attempt {attempt} for period {period}, retrying...", attempt, period);
                    await Task.Delay(TimeSpan.FromSeconds(2 * attempt), cancellationToken);
                    continue;
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Error executing ResetMonthlyFreezeShields for period {period}", period);
                    throw;
                }
            }
        }

        private static bool IsSerializationFailure(DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pex) return pex.SqlState == "40001";
            return false;
        }
    }
}
