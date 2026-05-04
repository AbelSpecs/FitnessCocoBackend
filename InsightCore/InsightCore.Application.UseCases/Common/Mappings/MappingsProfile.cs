using AutoMapper;
using InsightCore.Application.DTO;
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
        }
    }
}
