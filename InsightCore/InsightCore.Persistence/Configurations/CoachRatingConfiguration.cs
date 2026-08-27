using InsightCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsightCore.Persistence.Configurations
{
    public class CoachRatingConfiguration : IEntityTypeConfiguration<CoachRating>
    {
        public void Configure(EntityTypeBuilder<CoachRating> builder)
        {
            builder.ToTable("CoachRatings");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating)
                   .IsRequired();

            builder.Property(r => r.Comment)
                   .HasMaxLength(1000)
                   .HasColumnType("text");

            builder.Property(r => r.CreatedAt)
                   .IsRequired();

            // Indice unico compuesto: un alumno solo puede tener una valoracion activa por coach
            builder.HasIndex(r => new { r.CoachId, r.StudentId })
                   .IsUnique()
                   .HasDatabaseName("IX_CoachRatings_CoachId_StudentId");

            // Relacion con Coach
            builder.HasOne(r => r.Coach)
                   .WithMany(c => c.Ratings)
                   .HasForeignKey(r => r.CoachId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relacion con Student (sin cascade para preservar historial)
            builder.HasOne(r => r.Student)
                   .WithMany()
                   .HasForeignKey(r => r.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
