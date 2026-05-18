using InsightCore.Domain.Entities;
using InsightCore.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace InsightCore.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<CoachQRToken> CoachQRTokens { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<CoachStudent> CoachStudents { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<DailyStudentExercise>  DailyStudentExercises { get; set; }
        public DbSet<Gym> Gyms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Country>().ToTable("Countries");
            builder.Entity<City>().ToTable("Cities");
            builder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");

                entity.Property(p => p.Weight)
                      .HasPrecision(5, 2);

                entity.Property(p => p.Height)
                      .HasPrecision(4, 2);

                entity.Property(p => p.BodyFatPercentage)
                      .HasPrecision(5, 2);
            }); // El punto y coma va aquí al cerrar el bloque principal
            // Configuramos la clave primaria compuesta para la tabla intermedia
            builder.Entity<CoachStudent>()
                .HasKey(cs => new { cs.CoachId, cs.StudentId });
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<User>()
                .HasOne(u => u.Country)
                .WithMany()
                .HasForeignKey(u => u.CountryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<User>()
                .HasOne(u => u.City)
                .WithMany()
                .HasForeignKey(u => u.CityId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Student>()
            .HasOne<User>()
            .WithOne() // un usuario solo puede ser un estudiante
            .HasForeignKey<Student>(s => s.UserId);

            builder.Entity<Coach>()
                .HasOne<User>()
                .WithOne() // un usuario solo puede ser un coach
                .HasForeignKey<Coach>(c => c.UserId);

            // Configuración para convertir todas las propiedades DateTime a UTC
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                        v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
