using InsightCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsightCore.Persistence.Configurations
{
    /// <summary>
    /// Configuración Fluent API para la entidad Payment.
    /// Define tabla, columnas, índices y restricciones sin anotaciones en el dominio.
    /// </summary>
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            // Monto con precisión decimal de alta precisión (18 enteros, 4 decimales)
            builder.Property(p => p.Amount)
                   .IsRequired()
                   .HasPrecision(18, 4);

            // Enums almacenados como string para legibilidad en la BD
            builder.Property(p => p.Currency)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(10);

            builder.Property(p => p.PaymentMethod)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(30);

            builder.Property(p => p.PaymentDate)
                  .HasColumnType("timestamptz");

            builder.Property(p => p.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .HasDefaultValue(Domain.Enums.PaymentStatus.Pendiente);

            builder.Property(p => p.Reference)
                   .HasMaxLength(255);

            builder.Property(p => p.ReceiptKey)
                   .HasMaxLength(512);

            builder.Property(p => p.Notes)
                   .HasColumnType("text");

            builder.Property(p => p.CreatedAt)
                   .IsRequired()
                   .HasColumnType("timestamptz");

            builder.Property(p => p.UpdatedAt)
                   .HasColumnType("timestamptz");

            // ── Índices ──────────────────────────────────────────────────────────
            // Consultas frecuentes: por usuario, por alumno y por estado
            builder.HasIndex(p => p.UserId)
                   .HasDatabaseName("IX_Payments_UserId");

            //builder.HasIndex(p => p.StudentId)
            //       .HasDatabaseName("IX_Payments_StudentId");

            builder.HasIndex(p => p.Status)
                   .HasDatabaseName("IX_Payments_Status");

            // Índice compuesto para filtros combinados habituales
            builder.HasIndex(p => new { p.UserId, p.Status, p.CreatedAt })
                   .HasDatabaseName("IX_Payments_UserId_Status_CreatedAt");

            // ── Relaciones ───────────────────────────────────────────────────────
            builder.HasOne(p => p.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(p => p.Student)
            //       .WithMany()
            //       .HasForeignKey(p => p.StudentId)
            //       .IsRequired(false)
            //       .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
