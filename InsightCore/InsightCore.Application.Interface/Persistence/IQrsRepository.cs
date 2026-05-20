using InsightCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.Interface.Persistence
{
    public interface IQrsRepository 
    {
        Task<Gym> GetByIdAsync(int id);
        Task<CoachQRToken> InsertAsync(CoachQRToken qrToken);
        Task<bool> UpdateAsync(CoachQRToken qrToken);
        Task<bool> DeleteAsync(int id);
        Task DeactivateCoachTokensAsync(int coachId);
        Task<CoachQRToken> GetCoachTokensByTokenAsync(string token);
        Task<CoachQRToken> GetByCoachIdAsync(int coachId);
    }
}
