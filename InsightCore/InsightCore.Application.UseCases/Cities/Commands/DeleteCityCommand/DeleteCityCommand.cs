using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Cities.Commands.DeleteCityCommand
{
    public class DeleteCityCommand : IRequest<Response<CityDto>>
    {
        public int Id { get; set; }
    }
}
