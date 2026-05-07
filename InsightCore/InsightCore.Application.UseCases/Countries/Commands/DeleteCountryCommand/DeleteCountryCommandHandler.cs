using System;
using System.Threading;
using System.Threading.Tasks;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Countries.Commands.DeleteCountryCommand
{
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Response<CountryDto>>
    {
        private readonly IGenericRepository<InsightCore.Domain.Entities.Country> _genericRepository;

        public DeleteCountryCommandHandler(IGenericRepository<InsightCore.Domain.Entities.Country> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Response<CountryDto>> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<CountryDto>();
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
