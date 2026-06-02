using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Persistence.Repositories
{
    public class ExercisesRepository : IExercisesRepository
    {
        private readonly ApplicationDbContext _context;

        public ExercisesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Exercise> GetByIdAsync(int id)
        {
            return await _context.Set<Exercise>().Include(e => e.MuscleGroup).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await _context.Set<Exercise>().Include(e => e.MuscleGroup).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetByMuscleGroupIdAsync(int muscleGroupId)
        {
            return await _context.Set<Exercise>()
                .Include(e => e.MuscleGroup)
                .AsNoTracking()
                .Where(e => e.MuscleGroupId == muscleGroupId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetByCoachIdAsync(int coachId)
        {
            return await _context.Set<Exercise>()
                .Include(e => e.MuscleGroup)
                .AsNoTracking()
                .Where(e => (e.CoachId == null && !e.IsCustom) || e.CoachId == coachId)
                .ToListAsync();
        }

        public async Task<Exercise> InsertAsync(Exercise exercise)
        {
            await _context.Set<Exercise>().AddAsync(exercise);
            await _context.SaveChangesAsync();
            return exercise;
        }

        public async Task<bool> UpdateAsync(Exercise exercise)
        {
            _context.Set<Exercise>().Update(exercise);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Set<Exercise>().FindAsync(id);
            if (existing == null) return false;
            _context.Set<Exercise>().Remove(existing);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
    }
}