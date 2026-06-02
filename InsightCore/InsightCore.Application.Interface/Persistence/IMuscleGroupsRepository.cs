using InsightCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Persistence
{
    public interface IMuscleGroupsRepository
    {
        Task<MuscleGroup> GetByIdAsync(int id);
        Task<IEnumerable<MuscleGroup>> GetAllAsync();
        Task<MuscleGroup> InsertAsync(MuscleGroup entity);
        Task<bool> UpdateAsync(MuscleGroup entity);
        Task<bool> DeleteAsync(int id);
    }
}
