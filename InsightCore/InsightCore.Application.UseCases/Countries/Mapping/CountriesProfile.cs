using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Domain.Entities;

namespace InsightCore.Application.UseCases.Countries.Mapping
{
    public class CountriesProfile : Profile
    {
        public CountriesProfile()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
        }
    }
}
