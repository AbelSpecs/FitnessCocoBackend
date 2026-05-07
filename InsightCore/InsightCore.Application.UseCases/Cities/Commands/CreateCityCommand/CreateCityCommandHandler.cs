using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.UseCases.Countries.Commands.CreateCountryCommand;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Commands.CreateCityCommand
{
    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, Response<CityDto>>
    {
        private readonly IGenericRepository<City> _genericRepository;
        private readonly IMapper _mapper;

        public CreateCityCommandHandler(IGenericRepository<City> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<Response<CityDto>> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<CityDto>();
            try
            {
                var entity = _mapper.Map<City>(request.City);
                var inserted = await _genericRepository.InsertAsync(entity);
                if (!inserted)
                {
                    response.IsSuccess = false;
                    response.Message = "Could not create city.";
                    return response;
                }

                var dto = _mapper.Map<CityDto>(entity);
                response.Data = dto;
                response.IsSuccess = true;
                response.Message = "City created.";
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
