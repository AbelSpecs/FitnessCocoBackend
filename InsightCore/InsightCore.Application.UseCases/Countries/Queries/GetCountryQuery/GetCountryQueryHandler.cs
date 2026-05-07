using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Countries.Queries.GetCountryQuery
{
    public class GetCountryQueryHandler : IRequestHandler<GetCountryQuery, Response<CountryDto>>
    {
        private readonly IGenericRepository<Country> _genericRepository;
        private readonly IMapper _mapper;

        public GetCountryQueryHandler(IGenericRepository<Country> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<Response<CountryDto>> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<CountryDto>();
            try
            {
                var entity = await _genericRepository.GetAsync(request.Id.ToString());
                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Country not found.";
                    return response;
                }

                var dto = _mapper.Map<CountryDto>(entity);
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
