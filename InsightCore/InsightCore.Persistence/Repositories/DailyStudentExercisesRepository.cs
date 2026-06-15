using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Persistence.Repositories
{
    public class DailyStudentExercisesRepository : IDailyStudentExercisesRepository
    {
        private readonly ApplicationDbContext _context;

        public DailyStudentExercisesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DailyStudentExercise> GetByIdAsync(int id)
        {
            return await _context.Set<DailyStudentExercise>()
                .Include(d => d.Exercise)
                .Include(d => d.DailyExerciseSets)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DailyStudentExercise>> GetByStudentAsync(int studentId)
        {
            return await _context.Set<DailyStudentExercise>()
                .Include(d => d.Exercise)
                .Include(d => d.DailyExerciseSets)
                .AsNoTracking()
                .Where(d => d.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DailyStudentExercise>> GetByStudentAndDateAsync(int studentId, DateTime date)
        {
            // Compare Date only since ScheduledDate is stored as date type
            return await _context.Set<DailyStudentExercise>()
                .Include(d => d.Exercise).ThenInclude(e => e.MuscleGroup)
                .Include(d => d.DailyExerciseSets)
                .AsNoTracking()
                .Where(d => d.StudentId == studentId && d.ScheduledDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<DailyStudentExercise>> GetByStudentAndDateStartAndEndAsync(int studentId, DateTime dateStart, DateTime dateEnd)
        {
            // Compare Date only since ScheduledDate is stored as date type
            return await _context.Set<DailyStudentExercise>()
                .Include(d => d.Exercise).ThenInclude(e => e.MuscleGroup)
                .Include(d => d.DailyExerciseSets)
                .AsNoTracking()
                .Where(d => d.StudentId == studentId && d.ScheduledDate.Date >= dateStart.Date && d.ScheduledDate.Date <= dateEnd.Date)
                .ToListAsync();
        }


        public async Task<DailyStudentExercise> InsertAsync(DailyStudentExercise entity)
        {
            // To avoid issues assigning FK values when children arrive with default IDs,
            // save parent first, then assign parent's Id to children and save them.
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var children = entity.DailyExerciseSets != null ? entity.DailyExerciseSets.ToList() : null;

                    // detach children before inserting parent
                    entity.DailyExerciseSets = new List<DailyExerciseSet>();

                    await _context.Set<DailyStudentExercise>().AddAsync(entity);
                    await _context.SaveChangesAsync();

                    if (children != null && children.Any())
                    {
                        foreach (var c in children)
                        {
                            c.DailyStudentExerciseId = entity.Id;
                            // ensure navigation points to parent
                            c.DailyStudentExercise = entity;
                            await _context.Set<DailyExerciseSet>().AddAsync(c);
                        }
                        await _context.SaveChangesAsync();
                        entity.DailyExerciseSets = children;
                    }

                    await transaction.CommitAsync();
                    return entity;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> UpdateAsync(DailyStudentExercise entity)
        {
            _context.Set<DailyStudentExercise>().Update(entity);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Set<DailyStudentExercise>().FindAsync(id);
            if (existing == null) return false;
            _context.Set<DailyStudentExercise>().Remove(existing);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
    }
}
