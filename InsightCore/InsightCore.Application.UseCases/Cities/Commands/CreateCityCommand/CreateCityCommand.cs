using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Commands.CreateCityCommand
{
    public class CreateCityCommand : IRequest<Response<CityDto>>
    {
        public CityDto City { get; set; } = null!;
    }
}
