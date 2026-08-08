using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PyrosFit.Domain.Entities;

namespace PyrosFit.Infrastructure.Persistence.Configurations
{
    public class StreakLogConfiguration : IEntityTypeConfiguration<StreakLog>
    {
        public void Configure(EntityTypeBuilder<StreakLog> builder)
        {
            builder.ToTable("StreakLogs");
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id).HasColumnName("Id");
            builder.Property(l => l.StudentId).HasColumnName("StudentId");
            builder.Property(l => l.ActivityTypeId).HasColumnName("ActivityTypeId").HasColumnType("smallint");
            builder.Property(l => l.ActivityDate).HasColumnName("ActivityDate").HasColumnType("date");
            builder.Property(l => l.CreatedAt).HasColumnName("CreatedAt").HasColumnType("timestamp without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
