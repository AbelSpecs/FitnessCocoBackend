using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Domain.Entities;

namespace InsightCore.Application.UseCases
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Coach, CoachDto>().ReverseMap();
            // existing mappings can go here
        }
    }
}
