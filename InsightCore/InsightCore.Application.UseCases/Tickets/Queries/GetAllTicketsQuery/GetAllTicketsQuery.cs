using InsightCore.Application.DTO.Tickets;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Tickets.Queries.GetAllTicketsQuery
{
    public record GetAllTicketsQuery(bool? IsAddressed) : IRequest<Response<List<TicketDto>>>;
}

