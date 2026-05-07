using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InsightCore.Application.UseCases.Countries.Queries.GetCountriesQuery
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, Response<IEnumerable<CountryDto>>>
    {
        private readonly IGenericRepository<Country> _genericRepository;
        private readonly IMapper _mapper;

        public GetCountriesQueryHandler(IGenericRepository<Country> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<CountryDto>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<CountryDto>>();
            try
            {
                var entities = await _genericRepository.GetAllAsync();
                if (entities == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No countries found.";
                    return response;
                }

                var list = entities.ToList();
                var dtos = _mapper.Map<IEnumerable<CountryDto>>(list);

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
