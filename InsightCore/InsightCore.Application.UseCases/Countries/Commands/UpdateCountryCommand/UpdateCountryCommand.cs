using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Countries.Commands.UpdateCountryCommand
{
    public class UpdateCountryCommand : IRequest<Response<CountryDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
