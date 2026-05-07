using InsightCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Persistence
{
    public interface IGymsRepository
    {
        Task<Gym> GetByIdAsync(int id);
        Task<IEnumerable<Gym>> GetAllAsync();
        Task<Gym> InsertAsync(Gym gym);
        Task<bool> UpdateAsync(Gym gym);
        Task<bool> DeleteAsync(int id);
    }
}
