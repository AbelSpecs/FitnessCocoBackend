using InsightCore.Domain.Entities;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Persistence
{
    public interface ICoachRatingsRepository
    {
        /// <summary>
        /// Obtiene la valoracion existente de un alumno para un entrenador, o null si no existe.
        /// </summary>
        Task<CoachRating?> GetByCoachAndStudentAsync(int coachId, int studentId);

        /// <summary>
        /// Inserta una nueva valoracion o actualiza la existente (upsert por CoachId+StudentId).
        /// Retorna la entidad persistida y un flag indicando si fue actualizacion.
        /// </summary>
        Task<(CoachRating Rating, bool WasUpdated)> UpsertAsync(CoachRating rating);

        /// <summary>
        /// Calcula las estadisticas de valoracion del entrenador.
        /// </summary>
        Task<(double AverageRating, int TotalCount)> GetCoachRatingStatsAsync(int coachId);
    }
}
