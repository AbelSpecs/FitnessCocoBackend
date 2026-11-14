using InsightCore.Application.DTO.Tickets;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Tickets.Queries.GetMyTicketsQuery
{
    public record GetMyTicketsQuery(int UserId) : IRequest<Response<List<TicketDto>>>;
}

