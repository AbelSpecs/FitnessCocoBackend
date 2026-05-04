using InsightCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Persistence
{
    public interface ICoachesRepository
    {
        Task<Coach> GetByIdAsync(int id);
        Task<Coach> GetByUserIdAsync(int userId);
        Task<Coach> InsertAsync(Coach coach);
        Task<bool> UpdateAsync(Coach coach);
    }
}
