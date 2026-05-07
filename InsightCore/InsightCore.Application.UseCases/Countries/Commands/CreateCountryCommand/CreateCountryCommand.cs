using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Countries.Commands.CreateCountryCommand
{
    public class CreateCountryCommand : IRequest<Response<CountryDto>>
    {
        public CountryDto Country { get; set; } = null!;
    }
}
