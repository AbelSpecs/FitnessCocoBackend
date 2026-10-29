using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Cities.Commands.CreateCityCommand;
using InsightCore.Application.UseCases.Cities.Commands.UpdateCityCommand;
using InsightCore.Application.UseCases.CoachStudents.Commands.AssignStudentCommand;
using InsightCore.Application.UseCases.Countries.Commands.CreateCountryCommand;
using InsightCore.Application.UseCases.Countries.Commands.UpdateCountryCommand;
using InsightCore.Application.UseCases.DailyStudentExercises.Commands.UpdateDailyExerciseCommand;
using InsightCore.Application.UseCases.Exercises.Commands.CreateExerciseCommand;
using InsightCore.Application.UseCases.Students.Queries.GetStudentQuery;
using InsightCore.Application.UseCases.Users.Commands.RegisterUserCommand;
using InsightCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Common.Mappings
{
    public class MappingsProfile : Profile
    {
        
        public MappingsProfile()
        {

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, RegisterUserCommand>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<Coach, CoachDto>().ReverseMap();
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.BiologicalSex, opt => opt.MapFrom(src => src.BiologicalSex.HasValue ? src.BiologicalSex.Value.ToString() : null))
                .ForMember(dest => dest.AnthropometricMetrics, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.BiologicalSex, opt => opt.Ignore());
            CreateMap<Student, CreateOrUpdateStudentDto>().ReverseMap();
            CreateMap<Exercise, ExerciseDto>()
                .ForMember(dest => dest.MuscleGroup, opt => opt.MapFrom(src => src.MuscleGroup != null ? src.MuscleGroup.Name : null))
                .ForMember(dest => dest.MuscleGroupId, opt => opt.MapFrom(src => src.MuscleGroupId))
                .ReverseMap()
                .ForMember(dest => dest.MuscleGroup, opt => opt.Ignore())
                .ForMember(dest => dest.MuscleGroupId, opt => opt.MapFrom(src => src.MuscleGroupId));
            CreateMap<MuscleGroup, MuscleGroupDto>().ReverseMap();
            CreateMap<DailyExerciseSet, DailyExerciseSetDto>().ReverseMap();
            CreateMap<DailyStudentExercise, AssignDailyExerciseDto>()
                .ForMember(dest => dest.DailyExerciseSets, opt => opt.MapFrom(src => src.DailyExerciseSets))
                .ReverseMap()
                .ForMember(dest => dest.DailyExerciseSets, opt => opt.Ignore());
            CreateMap<Gym, GymDto>().ReverseMap();
            CreateMap<DailyStudentExercise, AssignDailyExerciseDto>().ReverseMap();
            CreateMap<Country, UpdateCountryCommand>().ReverseMap();
            CreateMap<City, CreateCityCommand>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, UpdateCityCommand>().ReverseMap();
            CreateMap<CoachQRToken, CoachQRTokenDto>().ReverseMap();
            CreateMap<CoachQRToken, GetStudentQuery>().ReverseMap();
            CreateMap<CountryDto,Country>().ReverseMap();
            CreateMap<Country, CreateCountryCommand>().ReverseMap();            
            CreateMap<Coach, UserDetailsDto>().ReverseMap();
            CreateMap<Coach, CoachDetailDto>().ReverseMap();
            CreateMap<Domain.Entities.CoachRating, RateCoachDto>().ReverseMap();
            CreateMap<Student, StudentDetailDto>().ReverseMap();
            CreateMap<User, StudentDetailDto>().ReverseMap();
            CreateMap<Student, UserDetailsDto>().ReverseMap();
            CreateMap<CoachStudent, AssignStudentCommand>().ReverseMap();
            CreateMap<CoachStudent, CoachStudentDto>().ReverseMap();
            CreateMap<CreateExerciseCommand, Exercise>().ReverseMap();
            CreateMap<ExerciseCreateDto, Exercise>().ReverseMap();
            CreateMap<UpdateDailyExerciseCommand, DailyStudentExercise>().ReverseMap();
            CreateMap<ExerciseUpdateDto, Exercise>().ReverseMap();

            // ── Pagos ──────────────────────────────────────────────────────────────
            // Entidad → DTO de lectura: enums se proyectan como strings legibles
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.Currency,      opt => opt.MapFrom(src => src.Currency.ToString()))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.Status,        opt => opt.MapFrom(src => src.Status.ToString()))
                // ReceiptUrl se construye fuera del mapper (p.ej. firmando la key en R2)
                .ForMember(dest => dest.ReceiptUrl,    opt => opt.Ignore());

            // DTO de escritura (ints) → Entidad (enums)
            // AutoMapper convierte int → enum automáticamente cuando los valores coinciden
            CreateMap<CreateOrUpdatePaymentDto, Payment>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => (InsightCore.Domain.Enums.PaymentCurrency)src.Currency))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => (InsightCore.Domain.Enums.PaymentMethod)src.PaymentMethod))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (InsightCore.Domain.Enums.PaymentStatus)src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
                //.ForMember(dest => dest.Student,       opt => opt.Ignore());

        }
    }
}
