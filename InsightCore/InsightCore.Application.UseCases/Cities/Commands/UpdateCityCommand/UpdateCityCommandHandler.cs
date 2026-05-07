using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.UseCases.Countries.Commands.UpdateCountryCommand;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Commands.UpdateCityCommand
{
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, Response<CityDto>>
    {
        private readonly IGenericRepository<City> _genericRepository;
        private readonly IMapper _mapper;

        public UpdateCityCommandHandler(IGenericRepository<City> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<Response<CityDto>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<CityDto>();
            try
            {
                var exists = await _genericRepository.GetAsync(request.Id.ToString());
                if (exists == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Country not found.";
                    return response;
                }

                var entity = _mapper.Map<City>(request);
                entity.Id = request.Id;

                var updated = await _genericRepository.UpdateAsync(entity);
                if (!updated)
                {
                    response.IsSuccess = false;
                    response.Message = "Could not update country.";
                    return response;
                }

                var dto = _mapper.Map<CityDto>(entity);
                response.Data = dto;
                response.IsSuccess = true;
                response.Message = "City updated.";
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
