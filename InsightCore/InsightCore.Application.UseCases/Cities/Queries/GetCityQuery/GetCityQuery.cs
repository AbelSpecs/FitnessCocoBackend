using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Queries.GetCityQuery
{
    public class GetCityQuery : IRequest<Response<CityDto>>
    {
        public int Id { get; set; }
    }
}
