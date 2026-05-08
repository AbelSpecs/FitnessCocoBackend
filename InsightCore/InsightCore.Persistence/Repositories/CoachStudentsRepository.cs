using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace InsightCore.Persistence.Repositories
{
    public class CoachStudentsRepository : ICoachStudentsRepository
    {
        private readonly ApplicationDbContext _context;

        public CoachStudentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CoachStudent> GetByIdsAsync(int coachId, int studentId)
        {
            return await _context.Set<CoachStudent>().AsNoTracking().FirstOrDefaultAsync(cs => cs.CoachId == coachId && cs.StudentId == studentId);
        }

        public async Task<CoachStudent> InsertAsync(CoachStudent entity)
        {
            await _context.Set<CoachStudent>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(CoachStudent entity)
        {
            _context.Set<CoachStudent>().Update(entity);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int coachId, int studentId)
        {
            var existing = await _context.Set<CoachStudent>().FindAsync(coachId, studentId);
            if (existing == null) return false;
            _context.Set<CoachStudent>().Remove(existing);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<IEnumerable<CoachStudent>> GetByCoachAsync(int coachId)
        {
            return await _context.Set<CoachStudent>().AsNoTracking().Where(cs => cs.CoachId == coachId).ToListAsync();
        }

        public async Task<CoachStudent> GetByStudentAsync(int studentId)
        {
            return await _context.Set<CoachStudent>().AsNoTracking().FirstOrDefaultAsync(cs => cs.StudentId == studentId);
        }
    }
}
