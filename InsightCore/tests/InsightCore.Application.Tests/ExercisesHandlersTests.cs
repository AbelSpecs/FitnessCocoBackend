using AutoMapper;
using InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByCoachQuery;
using InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByMuscleGroupQuery;
using InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByMuscleNameQuery;
using InsightCore.Application.DTO;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using InsightCore.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InsightCore.Application.Tests
{
    public class ExercisesHandlersTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ExercisesRepository _repo;
        private readonly IMapper _mapper;

        public ExercisesHandlersTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options, null);
            _repo = new ExercisesRepository(_context);

            // Configure minimal AutoMapper for mapping Exercise->ExerciseDto
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Exercise, ExerciseDto>()
                    .ForMember(d => d.MuscleGroup, opt => opt.MapFrom(s => s.MuscleGroup != null ? s.MuscleGroup.Name : null))
                    .ForMember(d => d.MuscleGroupId, opt => opt.MapFrom(s => s.MuscleGroupId));
            });
            _mapper = config.CreateMapper();

            Seed();
        }

        private void Seed()
        {
            var mg1 = new MuscleGroup { Name = "Chest" };
            var mg2 = new MuscleGroup { Name = "Back" };
            _context.MuscleGroups.AddRange(mg1, mg2);
            _context.SaveChanges();

            var ex1 = new Exercise { Name = "Push Up", MuscleGroupId = mg1.Id, IsCustom = false };
            var ex2 = new Exercise { Name = "Bench Press", MuscleGroupId = mg1.Id, CoachId = 5, IsCustom = true };
            var ex3 = new Exercise { Name = "Pull Up", MuscleGroupId = mg2.Id, IsCustom = false };
            _context.Exercises.AddRange(ex1, ex2, ex3);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetByMuscleGroupId_ReturnsCorrectExercises()
        {
            var handler = new GetExercisesByMuscleGroupQueryHandler(_repo, _mapper);
            var response = await handler.Handle(new GetExercisesByMuscleGroupQuery { MuscleGroupId = 1 }, default);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.Count());
        }

        [Fact]
        public async Task GetByCoachId_ReturnsGlobalAndCoachExercises()
        {
            var handler = new GetExercisesByCoachQueryHandler(_repo, _mapper);
            var response = await handler.Handle(new GetExercisesByCoachQuery { CoachId = 5 }, default);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Data);
            // Should include global exercises (Push Up, Pull Up) + coach's Bench Press = 3
            Assert.Equal(3, response.Data.Count());
        }

        [Fact]
        public async Task GetByMuscleName_ReturnsMatches()
        {
            var handler = new GetExercisesByMuscleNameQueryHandler(_repo, _mapper);
            var response = await handler.Handle(new GetExercisesByMuscleNameQuery { Name = "chest" }, default);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.Count());
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
