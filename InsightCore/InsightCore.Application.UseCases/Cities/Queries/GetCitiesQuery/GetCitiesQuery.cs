using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Queries.GetCitiesQuery
{
    public class GetCitiesQuery : IRequest<Response<IEnumerable<CityDto>>>
    {
    }
}
