using InsightCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Persistence
{
    public interface IDailyStudentExercisesRepository
    {
        Task<DailyStudentExercise> GetByIdAsync(int id);
        Task<IEnumerable<DailyStudentExercise>> GetByStudentAsync(int studentId);
        Task<IEnumerable<DailyStudentExercise>> GetByStudentAndDateAsync(int studentId, DateTime date);
        Task<IEnumerable<DailyStudentExercise>> GetByStudentAndDateStartAndEndAsync(int studentId, DateTime dateStart, DateTime dateEnd);
        Task<DailyStudentExercise> InsertAsync(DailyStudentExercise entity);
        Task<bool> UpdateAsync(DailyStudentExercise entity);
        Task<bool> DeleteAsync(int id);
    }
}