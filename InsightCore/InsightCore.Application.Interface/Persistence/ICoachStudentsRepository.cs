using InsightCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Persistence
{
    public interface ICoachStudentsRepository
    {
        Task<CoachStudent> GetByIdsAsync(int coachId, int studentId);
        Task<CoachStudent> InsertAsync(CoachStudent entity);
        Task<bool> UpdateAsync(CoachStudent entity);
        Task<bool> DeleteAsync(int coachId, int studentId);
        Task<IEnumerable<CoachStudent>> GetByCoachAsync(int coachId);
        Task<IEnumerable<CoachStudent>> GetByStudentAsync(int studentId);
    }
}
