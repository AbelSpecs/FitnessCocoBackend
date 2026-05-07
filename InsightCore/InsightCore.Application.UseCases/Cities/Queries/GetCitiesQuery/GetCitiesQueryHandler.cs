using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.UseCases.Countries.Queries.GetCountriesQuery;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Queries.GetCitiesQuery
{
    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, Response<IEnumerable<CityDto>>>
    {
        private readonly IGenericRepository<City> _genericRepository;
        private readonly IMapper _mapper;

        public GetCitiesQueryHandler(IGenericRepository<City> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<CityDto>>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<CityDto>>();
            try
            {
                var entities = await _genericRepository.GetAllAsync();
                if (entities == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No cities found.";
                    return response;
                }

                var list = entities.ToList();
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


