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
            return await _context.Set<DailyStudentExercise>().AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DailyStudentExercise>> GetByStudentAsync(int studentId)
        {
            return await _context.Set<DailyStudentExercise>().AsNoTracking().Where(d => d.StudentId == studentId).ToListAsync();
        }

        public async Task<DailyStudentExercise> InsertAsync(DailyStudentExercise entity)
        {
            await _context.Set<DailyStudentExercise>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
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
