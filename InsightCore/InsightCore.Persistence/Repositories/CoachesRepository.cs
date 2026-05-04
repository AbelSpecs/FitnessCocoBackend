using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

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
    }
}
