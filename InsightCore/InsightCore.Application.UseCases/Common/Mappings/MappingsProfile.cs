using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Cities.Commands.CreateCityCommand;
using InsightCore.Application.UseCases.Cities.Commands.UpdateCityCommand;
using InsightCore.Application.UseCases.CoachStudents.Commands.AssignStudentCommand;
using InsightCore.Application.UseCases.Countries.Commands.CreateCountryCommand;
using InsightCore.Application.UseCases.Countries.Commands.UpdateCountryCommand;
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
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, CreateOrUpdateStudentDto>().ReverseMap();
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, CreateOrUpdateStudentDto>().ReverseMap();
            CreateMap<Exercise, ExerciseDto>()
                .ForMember(dest => dest.MuscleGroup, opt => opt.MapFrom(src => src.MuscleGroup != null ? src.MuscleGroup.Name : null))
                .ForMember(dest => dest.MuscleGroupId, opt => opt.MapFrom(src => src.MuscleGroupId))
                .ReverseMap()
                .ForMember(dest => dest.MuscleGroup, opt => opt.Ignore())
                .ForMember(dest => dest.MuscleGroupId, opt => opt.MapFrom(src => src.MuscleGroupId));
            CreateMap<MuscleGroup, MuscleGroupDto>().ReverseMap();
            CreateMap<DailyExerciseSet, DailyExerciseSetDto>().ReverseMap();
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
            CreateMap<Student, StudentDetailDto>().ReverseMap();
            CreateMap<User, StudentDetailDto>().ReverseMap();
            CreateMap<Student, UserDetailsDto>().ReverseMap();
            CreateMap<CoachStudent, AssignStudentCommand>().ReverseMap();
            CreateMap<CoachStudent, CoachStudentDto>().ReverseMap();

        }
    }
}
