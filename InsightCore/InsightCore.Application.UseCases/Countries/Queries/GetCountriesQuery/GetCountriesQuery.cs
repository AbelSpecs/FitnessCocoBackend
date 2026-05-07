using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;

namespace InsightCore.Application.UseCases.Countries.Queries.GetCountriesQuery
{
    public class GetCountriesQuery : IRequest<Response<IEnumerable<CountryDto>>>
    {
    }
}
