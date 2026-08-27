using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using InsightCore.Application.DTO;
using PyrosFit.Domain.Entities;

namespace InsightCore.Persistence.Repositories
{
    public class CoachesRepository : ICoachesRepository
    {
        private readonly ApplicationDbContext _context;

        public CoachesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Coach> GetByIdAsync(int id)
        {
            return await _context.Set<Coach>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Coach> GetByUserIdAsync(int userId)
        {
            return await _context.Set<Coach>().AsNoTracking().FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Coach> InsertAsync(Coach coach)
        {
            await _context.Set<Coach>().AddAsync(coach);
            await _context.SaveChangesAsync();
            return coach;
        }

        public async Task<bool> UpdateAsync(Coach coach)
        {
            _context.Set<Coach>().Update(coach);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<CoachMetricsDto> GetCoachMetricsAsync(int coachId, int activeThresholdDays = 30)
        {
            // Umbral para considerar actividad reciente
            var threshold = DateTime.UtcNow.AddDays(-activeThresholdDays);

            // Total de alumnos asignados y activos mediante subconsultas eficientes
            // Ejecutar las consultas de forma secuencial para evitar operaciones concurrentes sobre el mismo DbContext
            var total = await _context.Set<CoachStudent>()
                .AsNoTracking()
                .Where(cs => cs.CoachId == coachId && cs.Status)
                .Select(cs => cs.StudentId)
                .Distinct()
                .CountAsync();

            // Alumnos activos: aquellos con StreakLog en el rango de threshold
            var active = await _context.Set<CoachStudent>()
                .AsNoTracking()
                .Where(cs => cs.CoachId == coachId && cs.Status)
                .Join(
                    _context.Set<StreakLog>().AsNoTracking().Where(sl => sl.ActivityDate >= threshold),
                    cs => cs.StudentId,
                    sl => sl.StudentId,
                    (cs, sl) => cs.StudentId
                )
                .Distinct()
                .CountAsync();

            // Total de rutinas/ejercicios creados por el coach (usar Exercises como proxy de "rutinas creadas")
            var routines = await _context.Set<Exercise>()
                .AsNoTracking()
                .CountAsync(e => e.CoachId == coachId);

            return new CoachMetricsDto
            {
                TotalStudents = total,
                ActiveStudents = active,
                InactiveStudents = Math.Max(0, total - active),
                TotalRoutinesCreated = routines
            };
        }
    }
}
