using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PyrosFit.Domain.Entities;

namespace PyrosFit.Infrastructure.Persistence.Configurations
{
    public class StudentStreakConfiguration : IEntityTypeConfiguration<StudentStreak>
    {
        public void Configure(EntityTypeBuilder<StudentStreak> builder)
        {
            builder.ToTable("StudentStreaks");
            builder.HasKey(s => s.StudentId);

            builder.Property(s => s.StudentId).HasColumnName("StudentId");
            builder.Property(s => s.CurrentStreak).HasColumnName("CurrentStreak").HasDefaultValue(0);
            builder.Property(s => s.LongestStreak).HasColumnName("LongestStreak").HasDefaultValue(0);
            builder.Property(s => s.LastCompletedDate).HasColumnName("LastCompletedDate").HasColumnType("date");
            builder.Property(s => s.FreezeShieldsAvailable).HasColumnName("FreezeShieldsAvailable").HasDefaultValue(2);
            builder.Property(s => s.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("timestamp without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
