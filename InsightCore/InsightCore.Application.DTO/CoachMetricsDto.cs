namespace InsightCore.Application.DTO
{
    /// <summary>
    /// Metricas agregadas del entrenador: alumnos y rutinas.
    /// Calculadas en la capa de persistencia mediante queries optimizadas con AsNoTracking.
    /// </summary>
    public class CoachMetricsDto
    {
        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }
        public int InactiveStudents { get; set; }
        public int TotalRoutinesCreated { get; set; }
    }
}
