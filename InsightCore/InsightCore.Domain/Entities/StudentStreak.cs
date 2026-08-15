using PyrosFit.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PyrosFit.Domain.Entities
{
    [Table("StudentStreaks")]
    public class StudentStreak
    {
        [Column("StudentId")]
        public int StudentId { get; set; }

        [Column("CurrentStreak")]
        public int CurrentStreak { get; private set; } = 0;

        [Column("LongestStreak")]
        public int LongestStreak { get; private set; } = 0;

        [Column("LastCompletedDate")]
        public DateTime? LastCompletedDate { get; private set; }

        [Column("FreezeShieldsAvailable")]
        public int FreezeShieldsAvailable { get; private set; } = 2;

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; private set; }

        // Método de dominio que aplica la lógica de racha y devuelve logs a persistir
        public List<StreakLog> RecordActivity(DateTime activityDate)
        {
            var logs = new List<StreakLog>();
            var actDate = activityDate.Date;

            if (LastCompletedDate.HasValue && LastCompletedDate.Value.Date == actDate)
            {
                // misma fecha: ignorar
                return logs;
            }

            if (!LastCompletedDate.HasValue)
            {
                CurrentStreak = 1;
                LongestStreak = Math.Max(LongestStreak, CurrentStreak);
                LastCompletedDate = actDate;
                UpdatedAt = DateTime.Now;
                logs.Add(new StreakLog { StudentId = StudentId, ActivityTypeId = (short)StreakActivityType.WorkoutCompleted, ActivityDate = actDate, CreatedAt = DateTime.Now });
                return logs;
            }

            var daysDiff = (actDate - LastCompletedDate.Value.Date).Days;

            if (daysDiff == 1)
            {
                CurrentStreak += 1;
                if (CurrentStreak > LongestStreak) LongestStreak = CurrentStreak;
                LastCompletedDate = actDate;
                UpdatedAt = DateTime.Now;
                logs.Add(new StreakLog { StudentId = StudentId, ActivityTypeId = (short)StreakActivityType.WorkoutCompleted, ActivityDate = actDate, CreatedAt = DateTime.Now });
                return logs;
            }

            // salto de 1 día: permitir escudo
            if (daysDiff == 2 && FreezeShieldsAvailable > 0)
            {
                FreezeShieldsAvailable -= 1;
                // registrar uso de escudo y la actividad
                logs.Add(new StreakLog { StudentId = StudentId, ActivityTypeId = (short)StreakActivityType.FreezeShieldUsed, ActivityDate = actDate, CreatedAt = DateTime.Now });
                CurrentStreak += 1;
                if (CurrentStreak > LongestStreak) LongestStreak = CurrentStreak;
                LastCompletedDate = actDate;
                UpdatedAt = DateTime.Now;
                logs.Add(new StreakLog { StudentId = StudentId, ActivityTypeId = (short)StreakActivityType.WorkoutCompleted, ActivityDate = actDate, CreatedAt = DateTime.Now });
                return logs;
            }

            // inactividad >= 2 días sin escudo: reiniciar racha a 1
            CurrentStreak = 1;
            if (CurrentStreak > LongestStreak) LongestStreak = CurrentStreak;
            LastCompletedDate = actDate;
            UpdatedAt = DateTime.Now;
            logs.Add(new StreakLog { StudentId = StudentId, ActivityTypeId = (short)StreakActivityType.StreakReset, ActivityDate = actDate, CreatedAt = DateTime.Now });
            logs.Add(new StreakLog { StudentId = StudentId, ActivityTypeId = (short)StreakActivityType.WorkoutCompleted, ActivityDate = actDate, CreatedAt = DateTime.Now });
            return logs;
        }

        public StreakLog? UseFreezeShield(DateTime? shieldDate = null)
        {
            if (FreezeShieldsAvailable <= 0) return null;

            var targetDate = shieldDate?.Date ?? DateTime.UtcNow.Date;
            FreezeShieldsAvailable -= 1;
            UpdatedAt = DateTime.UtcNow;

            return new StreakLog
            {
                StudentId = StudentId,
                ActivityTypeId = (short)StreakActivityType.FreezeShieldUsed,
                ActivityDate = targetDate,
                CreatedAt = DateTime.UtcNow
            };
        }

        public StreakLog AdjustStreak(int? currentStreak, int? longestStreak, int? freezeShields)
        {
            if (currentStreak.HasValue && currentStreak.Value >= 0)
                CurrentStreak = currentStreak.Value;

            if (longestStreak.HasValue && longestStreak.Value >= 0)
                LongestStreak = longestStreak.Value;
            else if (CurrentStreak > LongestStreak)
                LongestStreak = CurrentStreak;

            if (freezeShields.HasValue && freezeShields.Value >= 0)
                FreezeShieldsAvailable = freezeShields.Value;

            UpdatedAt = DateTime.UtcNow;

            return new StreakLog
            {
                StudentId = StudentId,
                ActivityTypeId = (short)StreakActivityType.ManualAdjustment,
                ActivityDate = DateTime.UtcNow.Date,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void AddFreezeShields(int count)
        {
            if (count > 0)
            {
                FreezeShieldsAvailable += count;
                UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
