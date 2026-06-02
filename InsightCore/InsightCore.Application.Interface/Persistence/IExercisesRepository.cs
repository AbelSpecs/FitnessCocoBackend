using InsightCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Persistence
{
    public interface IExercisesRepository
    {
        Task<Exercise> GetByIdAsync(int id);
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task<IEnumerable<Exercise>> GetByMuscleGroupIdAsync(int muscleGroupId);
        Task<IEnumerable<Exercise>> GetByCoachIdAsync(int coachId);
        Task<Exercise> InsertAsync(Exercise exercise);
        Task<bool> UpdateAsync(Exercise exercise);
        Task<bool> DeleteAsync(int id);
    }
}