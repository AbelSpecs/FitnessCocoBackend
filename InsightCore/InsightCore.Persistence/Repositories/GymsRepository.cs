using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Persistence.Repositories
{
    public class GymsRepository : IGymsRepository
    {
        private readonly ApplicationDbContext _context;

        public GymsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Gym> GetByIdAsync(int id)
        {
            return await _context.Set<Gym>().AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Gym>> GetAllAsync()
        {
            return await _context.Set<Gym>().AsNoTracking().ToListAsync();
        }

        public async Task<Gym> InsertAsync(Gym gym)
        {
            await _context.Set<Gym>().AddAsync(gym);
            await _context.SaveChangesAsync();
            return gym;
        }

        public async Task<bool> UpdateAsync(Gym gym)
        {
            _context.Set<Gym>().Update(gym);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Set<Gym>().FindAsync(id);
            if (existing == null) return false;
            _context.Set<Gym>().Remove(existing);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
    }
}
