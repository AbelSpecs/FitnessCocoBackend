using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Countries.Commands.UpdateCountryCommand
{
    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Response<CountryDto>>
    {
        private readonly IGenericRepository<Country> _genericRepository;
        private readonly IMapper _mapper;

        public UpdateCountryCommandHandler(IGenericRepository<Country> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<Response<CountryDto>> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<CountryDto>();
            try
            {
                var exists = await _genericRepository.GetAsync(request.Id.ToString());
                if (exists == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Country not found.";
                    return response;
                }

                var entity = _mapper.Map<Country>(request);
                entity.Id = request.Id;

                var updated = await _genericRepository.UpdateAsync(entity);
                if (!updated)
                {
                    response.IsSuccess = false;
                    response.Message = "Could not update country.";
                    return response;
                }

                var dto = _mapper.Map<CountryDto>(entity);
                response.Data = dto;
                response.IsSuccess = true;
                response.Message = "Country updated.";
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
