using InsightCore.Application.DTO;
using InsightCore.Domain.Entities;
using InsightCore.Domain.Enums;

namespace InsightCore.Application.UseCases.Common.Services
{
    /// <summary>
    /// Servicio estático para calcular métricas antropométricas a partir de los datos
    /// físicos del alumno y la fecha de nacimiento del usuario asociado.
    /// 
    /// Todas las métricas son nullable: si faltan datos de entrada requeridos para
    /// un cálculo específico, ese campo devuelve null sin lanzar excepciones.
    /// </summary>
    public static class AnthropometricCalculatorService
    {
        // Factores de actividad para TDEE (Mifflin-St Jeor)
        private static readonly Dictionary<string, double> ActivityFactors = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Sedentary",        1.2   },
            { "LightlyActive",    1.375 },
            { "ModeratelyActive", 1.55  },
            { "VeryActive",       1.725 },
            { "ExtraActive",      1.9   }
        };

        /// <summary>
        /// Calcula el conjunto completo de métricas antropométricas.
        /// </summary>
        /// <param name="student">Entidad Student con datos físicos.</param>
        /// <param name="birthdate">Fecha de nacimiento del User asociado.</param>
        /// <returns>DTO con todas las métricas calculadas (campos null si faltan datos).</returns>
        public static AnthropometricMetricsDto Calculate(Student student, DateTime birthdate)
        {
            var metrics = new AnthropometricMetricsDto();

            // ─── IMC ─────────────────────────────────────────────────────────────────
            if (student.Weight.HasValue && student.Height.HasValue && student.Height > 0)
            {
                var bmi = student.Weight.Value / (student.Height.Value * student.Height.Value);
                metrics.Bmi = Math.Round(bmi, 2);
                metrics.BmiClassification = ClassifyBmi((double)bmi);
            }

            // ─── Masa grasa y masa magra ──────────────────────────────────────────────
            if (student.Weight.HasValue && student.BodyFatPercentage.HasValue)
            {
                var fatFraction = student.BodyFatPercentage.Value / 100m;
                metrics.FatMass      = Math.Round(student.Weight.Value * fatFraction, 2);
                metrics.LeanBodyMass = Math.Round(student.Weight.Value * (1m - fatFraction), 2);
            }

            // ─── Edad ─────────────────────────────────────────────────────────────────
            var today = DateTime.UtcNow;
            int age = today.Year - birthdate.Year;
            if (birthdate.Date > today.AddYears(-age)) age--;

            // ─── TMB (Mifflin-St Jeor) ────────────────────────────────────────────────
            // Requiere: peso, altura, edad y sexo biológico.
            // altura se almacena en metros; Mifflin usa centímetros.
            if (student.Weight.HasValue && student.Height.HasValue && student.BiologicalSex.HasValue)
            {
                double kg  = (double)student.Weight.Value;
                double cm  = (double)(student.Height.Value * 100m);
                double bmr;

                if (student.BiologicalSex == BiologicalSex.Male)
                    bmr = (10 * kg) + (6.25 * cm) - (5 * age) + 5;
                else if (student.BiologicalSex == BiologicalSex.Female)
                    bmr = (10 * kg) + (6.25 * cm) - (5 * age) - 161;
                else
                    // Para "Other" usamos el promedio de ambas fórmulas
                    bmr = (10 * kg) + (6.25 * cm) - (5 * age) - 78;

                metrics.BasalMetabolicRate = Math.Round((decimal)bmr, 0);

                // ─── TDEE ─────────────────────────────────────────────────────────────
                if (!string.IsNullOrWhiteSpace(student.ActivityLevel) &&
                    ActivityFactors.TryGetValue(student.ActivityLevel, out double factor))
                {
                    metrics.TotalDailyEnergyExpenditure = Math.Round((decimal)(bmr * factor), 0);
                }
            }

            // ─── Agua Corporal Total (Watson) ─────────────────────────────────────────
            // Requiere: peso, altura, sexo biológico.
            if (student.Weight.HasValue && student.Height.HasValue && student.BiologicalSex.HasValue)
            {
                double kg = (double)student.Weight.Value;
                double cm = (double)(student.Height.Value * 100m);
                double tbw;

                if (student.BiologicalSex == BiologicalSex.Male)
                    tbw = 2.447 - (0.09516 * age) + (0.1074 * cm) + (0.3362 * kg);
                else if (student.BiologicalSex == BiologicalSex.Female)
                    tbw = -2.097 + (0.1069 * cm) + (0.2466 * kg);
                else
                    // Promedio de ambas fórmulas para "Other"
                    tbw = ((2.447 - (0.09516 * age) + (0.1074 * cm) + (0.3362 * kg))
                         + (-2.097 + (0.1069 * cm) + (0.2466 * kg))) / 2;

                metrics.TotalBodyWater = Math.Round((decimal)tbw, 2);
            }

            return metrics;
        }

        // ─── Clasificación OMS del IMC ────────────────────────────────────────────────
        private static string ClassifyBmi(double bmi) => bmi switch
        {
            < 18.5 => "Bajo peso",
            < 25.0 => "Normal",
            < 30.0 => "Sobrepeso",
            < 35.0 => "Obesidad I",
            < 40.0 => "Obesidad II",
            _      => "Obesidad III"
        };
    }
}
