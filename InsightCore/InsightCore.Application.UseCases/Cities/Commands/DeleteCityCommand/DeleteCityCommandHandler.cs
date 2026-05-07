using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.UseCases.Countries.Commands.DeleteCountryCommand;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Commands.DeleteCityCommand
{
    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, Response<CityDto>>
    {
        private readonly IGenericRepository<City> _genericRepository;

        public DeleteCityCommandHandler(IGenericRepository<City> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Response<CityDto>> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<CityDto>();
            try
            {
                var deleted = await _genericRepository.DeleteAsync(request.Id.ToString());
                if (!deleted)
                {
                    response.IsSuccess = false;
                    response.Message = "Country not found or could not be deleted.";
                    return response;
                }

                response.IsSuccess = true;
                response.Message = "Country deleted.";
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
