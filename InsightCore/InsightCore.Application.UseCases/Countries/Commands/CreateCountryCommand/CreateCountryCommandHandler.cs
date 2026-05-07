using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Countries.Commands.CreateCountryCommand
{
    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Response<CountryDto>>
    {
        private readonly IGenericRepository<Country> _genericRepository;
        private readonly IMapper _mapper;

        public CreateCountryCommandHandler(IGenericRepository<Country> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<Response<CountryDto>> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<CountryDto>();
            try
            {
                var entity = _mapper.Map<Country>(request.Country);
                var inserted = await _genericRepository.InsertAsync(entity);
                if (!inserted)
                {
                    response.IsSuccess = false;
                    response.Message = "Could not create country.";
                    return response;
                }

                var dto = _mapper.Map<CountryDto>(entity);
                response.Data = dto;
                response.IsSuccess = true;
                response.Message = "Country created.";
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
