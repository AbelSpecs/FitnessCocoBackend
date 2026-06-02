using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Persistence.Repositories
{
    public class DailyExerciseSetsRepository : IDailyExerciseSetsRepository
    {
        private readonly ApplicationDbContext _context;

        public DailyExerciseSetsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DailyExerciseSet> GetByIdAsync(int id)
        {
            return await _context.Set<DailyExerciseSet>().AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DailyExerciseSet>> GetAllAsync()
        {
            return await _context.Set<DailyExerciseSet>().AsNoTracking().ToListAsync();
        }

        public async Task<DailyExerciseSet> InsertAsync(DailyExerciseSet entity)
        {
            await _context.Set<DailyExerciseSet>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(DailyExerciseSet entity)
        {
            _context.Set<DailyExerciseSet>().Update(entity);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Set<DailyExerciseSet>().FindAsync(id);
            if (existing == null) return false;
            _context.Set<DailyExerciseSet>().Remove(existing);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
    }
}
