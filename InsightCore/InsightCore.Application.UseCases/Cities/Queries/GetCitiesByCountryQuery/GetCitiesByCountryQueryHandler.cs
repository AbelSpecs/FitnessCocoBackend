using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Queries.GetCitiesByCountryQuery
{
    public class GetCitiesByCountryQueryHandler : IRequestHandler<GetCitiesByCountryQuery, Response<IEnumerable<CityDto>>>
    {
        private readonly IGenericRepository<City> _genericRepository;
        private readonly ICitiesRepository _cityRepository;
        private readonly IMapper _mapper;

        public GetCitiesByCountryQueryHandler(IGenericRepository<City> genericRepository, ICitiesRepository cityRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<CityDto>>> Handle(GetCitiesByCountryQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<CityDto>>();
            try
            {
                var entities = await _cityRepository.GetCitiesByCountryAsync(request.CountryId.ToString());
                if (entities == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No cities found.";
                    return response;
                }
                var cities = entities.Where(c => c.CountryId == request.CountryId);
                var list = cities.ToList();
                var dtos = _mapper.Map<IEnumerable<CityDto>>(list);

                response.Data = dtos;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
