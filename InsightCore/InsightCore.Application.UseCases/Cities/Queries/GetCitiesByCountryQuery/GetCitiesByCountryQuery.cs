using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Queries.GetCitiesByCountryQuery
{
    public class GetCitiesByCountryQuery : IRequest<Response<IEnumerable<CityDto>>>
    {
        public int CountryId { get; set; }
    }
}
