using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.UseCases.Countries.Queries.GetCountryQuery;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Queries.GetCityQuery
{
    public class GetCityQueryHandler : IRequestHandler<GetCityQuery, Response<CityDto>>
    {
        private readonly IGenericRepository<City> _genericRepository;
        private readonly IMapper _mapper;

        public GetCityQueryHandler(IGenericRepository<City> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<Response<CityDto>> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<CityDto>();
            try
            {
                var entity =  _genericRepository.Get(request.Id.ToString());
                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.Message = "City not found.";
                    return response;
                }

                var dto = _mapper.Map<CityDto>(entity);
                response.Data = dto;
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
