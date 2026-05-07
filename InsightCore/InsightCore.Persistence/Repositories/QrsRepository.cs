using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Persistence.Repositories
{
    public class QrsRepository : IQrsRepository
    {
        private readonly ApplicationDbContext _context;

        public QrsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Gym> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CoachQRToken> InsertAsync(CoachQRToken qrToken)
        {
            await _context.Set<CoachQRToken>().AddAsync(qrToken);
            await _context.SaveChangesAsync();
            return qrToken;
        }

        public async Task<bool> UpdateAsync(CoachQRToken qrToken)
        {
            _context.Set<CoachQRToken>().Update(qrToken);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task DeactivateCoachTokensAsync(int coachId)
        {
            await _context.CoachQRTokens
                .Where(q => q.CoachId == coachId && q.IsActive)
                .ExecuteUpdateAsync(s => s.SetProperty(b => b.IsActive, false));
        }

        public async Task<CoachQRToken> GetCoachTokensByTokenAsync(string token)
        {
            return await _context.CoachQRTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Token == token && x.ExpiresAt > DateTime.UtcNow);
  
        }


    }
}
