using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Countries.Queries.GetCountryQuery
{
    public class GetCountryQuery : IRequest<Response<CountryDto>>
    {
        public int Id { get; set; }
    }
}
