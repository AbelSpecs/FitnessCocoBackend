using InsightCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Persistence
{
    public interface IDailyExerciseSetsRepository
    {
        Task<DailyExerciseSet> GetByIdAsync(int id);
        Task<IEnumerable<DailyExerciseSet>> GetAllAsync();
        Task<DailyExerciseSet> InsertAsync(DailyExerciseSet entity);
        Task<bool> UpdateAsync(DailyExerciseSet entity);
        Task<bool> DeleteAsync(int id);
    }
}
