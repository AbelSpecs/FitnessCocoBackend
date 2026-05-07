using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Countries.Commands.UpdateCountryCommand;
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
            CreateMap<Domain.Entities.Student, StudentDto>().ReverseMap();
            CreateMap<Domain.Entities.Student, CreateOrUpdateStudentDto>().ReverseMap();
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, CreateOrUpdateStudentDto>().ReverseMap();
            CreateMap<Exercise, ExerciseDto>().ReverseMap();
            CreateMap<Domain.Entities.Gym, GymDto>().ReverseMap();
            CreateMap<DailyStudentExercise, AssignDailyExerciseDto>().ReverseMap();
            CreateMap<Country, UpdateCountryCommand>().ReverseMap();
            

        }
    }
}
