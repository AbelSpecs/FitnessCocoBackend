using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Persistence.Repositories
{
    public class MuscleGroupsRepository : IMuscleGroupsRepository
    {
        private readonly ApplicationDbContext _context;

        public MuscleGroupsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MuscleGroup> GetByIdAsync(int id)
        {
            return await _context.Set<MuscleGroup>().AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MuscleGroup>> GetAllAsync()
        {
            return await _context.Set<MuscleGroup>().AsNoTracking().ToListAsync();
        }

        public async Task<MuscleGroup> InsertAsync(MuscleGroup entity)
        {
            await _context.Set<MuscleGroup>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(MuscleGroup entity)
        {
            _context.Set<MuscleGroup>().Update(entity);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Set<MuscleGroup>().FindAsync(id);
            if (existing == null) return false;
            // Prevent delete if has exercises
            var hasExercises = await _context.Set<Exercise>().AnyAsync(e => e.MuscleGroupId == id);
            if (hasExercises) return false;
            _context.Set<MuscleGroup>().Remove(existing);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
    }
}
