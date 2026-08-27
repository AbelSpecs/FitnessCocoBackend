using InsightCore.Application.DTO;
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

        /// <summary>
        /// Obtiene las metricas de alumnos y rutinas del entrenador en una sola consulta agregada.
        /// activeThresholdDays: dias hacia atras para considerar a un alumno activo por streak.
        /// </summary>
        Task<CoachMetricsDto> GetCoachMetricsAsync(int coachId, int activeThresholdDays = 30);
    }
}
