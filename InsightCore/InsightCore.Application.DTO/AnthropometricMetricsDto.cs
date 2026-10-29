namespace InsightCore.Application.DTO
{
    /// <summary>
    /// Métricas antropométricas calculadas en tiempo de consulta a partir de los datos físicos del alumno.
    /// Los campos son nullable: si faltan datos de entrada (ej. BodyFatPercentage no cargado),
    /// las métricas dependientes devuelven null sin lanzar errores.
    /// </summary>
    public class AnthropometricMetricsDto
    {
        /// <summary>Índice de Masa Corporal = peso(kg) / altura(m)²</summary>
        public decimal? Bmi { get; set; }

        /// <summary>Clasificación OMS del IMC: "Bajo peso", "Normal", "Sobrepeso", "Obesidad I", "Obesidad II", "Obesidad III"</summary>
        public string? BmiClassification { get; set; }

        /// <summary>Masa Magra (kg) = peso × (1 - %GrasaCorporal / 100)</summary>
        public decimal? LeanBodyMass { get; set; }

        /// <summary>Masa Grasa (kg) = peso × (%GrasaCorporal / 100)</summary>
        public decimal? FatMass { get; set; }

        /// <summary>Tasa Metabólica Basal (kcal/día) — Mifflin-St Jeor. Requiere peso, altura, edad y sexo biológico.</summary>
        public decimal? BasalMetabolicRate { get; set; }

        /// <summary>Gasto Energético Diario Total (kcal/día) = TMB × factor de actividad</summary>
        public decimal? TotalDailyEnergyExpenditure { get; set; }

        /// <summary>Agua Corporal Total estimada (litros) — Ecuación de Watson</summary>
        public decimal? TotalBodyWater { get; set; }
    }
}
