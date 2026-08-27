using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InsightCore.Persistence.Repositories
{
    public class CoachRatingsRepository : ICoachRatingsRepository
    {
        private readonly ApplicationDbContext _context;

        public CoachRatingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CoachRating?> GetByCoachAndStudentAsync(int coachId, int studentId)
        {
            return await _context.Set<CoachRating>()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.CoachId == coachId && r.StudentId == studentId);
        }

        public async Task<(CoachRating Rating, bool WasUpdated)> UpsertAsync(CoachRating incoming)
        {
            // Buscar valoracion existente (con tracking para poder actualizarla)
            var existing = await _context.Set<CoachRating>()
                .FirstOrDefaultAsync(r => r.CoachId == incoming.CoachId && r.StudentId == incoming.StudentId);

            if (existing is not null)
            {
                // Actualizar valoracion existente
                existing.Rating    = incoming.Rating;
                existing.Comment   = incoming.Comment;
                existing.UpdatedAt = DateTime.UtcNow;

                _context.Set<CoachRating>().Update(existing);
                await _context.SaveChangesAsync();

                return (existing, true);
            }
            else
            {
                // Insertar nueva valoracion
                incoming.CreatedAt = DateTime.UtcNow;
                await _context.Set<CoachRating>().AddAsync(incoming);
                await _context.SaveChangesAsync();

                return (incoming, false);
            }
        }

        public async Task<(double AverageRating, int TotalCount)> GetCoachRatingStatsAsync(int coachId)
        {
            var stats = await _context.Set<CoachRating>()
                .AsNoTracking()
                .Where(r => r.CoachId == coachId)
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    Total   = g.Count(),
                    Average = g.Average(r => (double)r.Rating)
                })
                .FirstOrDefaultAsync();

            if (stats is null)
                return (0.0, 0);

            var rounded = Math.Round(stats.Average, 1, MidpointRounding.AwayFromZero);
            return (rounded, stats.Total);
        }
    }
}
