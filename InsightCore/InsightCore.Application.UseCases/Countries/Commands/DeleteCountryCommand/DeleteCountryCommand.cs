using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Countries.Commands.DeleteCountryCommand
{
    public class DeleteCountryCommand : IRequest<Response<CountryDto>>
    {
        public int Id { get; set; }
    }
}
