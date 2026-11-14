using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InsightCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBiologicalSexToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "CoachQRTokens",
                columns: table => new
                {
                    CoachId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachQRTokens", x => x.CoachId);
                });

            migrationBuilder.CreateTable(
                name: "CoachStudents",
                columns: table => new
                {
                    CoachId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachStudents", x => new { x.CoachId, x.StudentId });
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PhoneCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gyms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<Point>(type: "geometry", nullable: false),
                    CheckInRadius = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gyms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MuscleGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StreakActivityTypes",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreakActivityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StreakLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    ActivityTypeId = table.Column<short>(type: "smallint", nullable: false),
                    ActivityDate = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreakLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentStreaks",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrentStreak = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LongestStreak = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LastCompletedDate = table.Column<DateTime>(type: "date", nullable: true),
                    FreezeShieldsAvailable = table.Column<int>(type: "integer", nullable: false, defaultValue: 2),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStreaks", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "SystemJobLogs",
                columns: table => new
                {
                    Period = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    JobName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemJobLogs", x => x.Period);
                });

            migrationBuilder.CreateTable(
                name: "transacciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_id = table.Column<int>(type: "integer", nullable: false),
                    fuente = table.Column<string>(type: "text", nullable: false),
                    monto = table.Column<decimal>(type: "numeric", nullable: false),
                    moneda = table.Column<string>(type: "text", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    categoria = table.Column<string>(type: "text", nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transacciones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoachId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MuscleGroupId = table.Column<int>(type: "integer", nullable: false),
                    VideoKey = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsCustom = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_MuscleGroups_MuscleGroupId",
                        column: x => x.MuscleGroupId,
                        principalTable: "MuscleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<int>(type: "integer", nullable: true),
                    CityId = table.Column<int>(type: "integer", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Birthdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Token = table.Column<string>(type: "text", nullable: true),
                    TokenExpiry = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EmailConfirmationToken = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmationTokenExpiry = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "text", nullable: true),
                    PasswordResetTokenExpiry = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ProfilePictureKey = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BannerPictureKey = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DailyStudentExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoachId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "date", nullable: false),
                    CoachNotes = table.Column<string>(type: "text", nullable: true),
                    StudentNotes = table.Column<string>(type: "text", nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyStudentExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyStudentExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    Certifications = table.Column<string>(type: "text", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coaches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Height = table.Column<decimal>(type: "numeric(3,2)", precision: 4, scale: 2, nullable: true),
                    BodyFatPercentage = table.Column<decimal>(type: "numeric(4,2)", precision: 5, scale: 2, nullable: true),
                    BiologicalSex = table.Column<short>(type: "smallint", nullable: true),
                    FitnessGoal = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ActivityLevel = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    MedicalConditions = table.Column<string>(type: "text", nullable: true),
                    Allergies = table.Column<string>(type: "text", nullable: true),
                    FitnessExperience = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    GeneralNotes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    GymId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyExerciseSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DailyStudentExerciseId = table.Column<int>(type: "integer", nullable: false),
                    SetNumber = table.Column<int>(type: "integer", nullable: false),
                    TargetReps = table.Column<int>(type: "integer", nullable: false),
                    TargetWeight = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    RestTime = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ActualReps = table.Column<int>(type: "integer", nullable: true),
                    ActualWeight = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    IsAchieved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyExerciseSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyExerciseSets_DailyStudentExercises_DailyStudentExercis~",
                        column: x => x.DailyStudentExerciseId,
                        principalTable: "DailyStudentExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoachRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoachId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoachRatings_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoachRatings_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "StreakActivityTypes",
                columns: new[] { "Id", "Code", "Description", "Name" },
                values: new object[,]
                {
                    { (short)1, "WORKOUT_COMPLETED", "Alumno completó un entrenamiento", "Workout Completed" },
                    { (short)2, "FREEZE_SHIELD_USED", "Se usó un escudo para mantener la racha", "Freeze Shield Used" },
                    { (short)3, "REST_DAY", "Día de descanso", "Rest Day" },
                    { (short)4, "STREAK_RESET", "Reinicio de racha", "Streak Reset" },
                    { (short)5, "MANUAL_ADJUSTMENT", "Ajuste manual de la racha", "Manual Adjustment" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_UserId",
                table: "Coaches",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoachRatings_CoachId_StudentId",
                table: "CoachRatings",
                columns: new[] { "CoachId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoachRatings_StudentId",
                table: "CoachRatings",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyExerciseSets_DailyStudentExerciseId",
                table: "DailyExerciseSets",
                column: "DailyStudentExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyStudentExercises_ExerciseId",
                table: "DailyStudentExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_MuscleGroupId",
                table: "Exercises",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CityId",
                table: "Users",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryId",
                table: "Users",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachQRTokens");

            migrationBuilder.DropTable(
                name: "CoachRatings");

            migrationBuilder.DropTable(
                name: "CoachStudents");

            migrationBuilder.DropTable(
                name: "DailyExerciseSets");

            migrationBuilder.DropTable(
                name: "Gyms");

            migrationBuilder.DropTable(
                name: "StreakActivityTypes");

            migrationBuilder.DropTable(
                name: "StreakLogs");

            migrationBuilder.DropTable(
                name: "StudentStreaks");

            migrationBuilder.DropTable(
                name: "SystemJobLogs");

            migrationBuilder.DropTable(
                name: "transacciones");

            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "DailyStudentExercises");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "MuscleGroups");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
