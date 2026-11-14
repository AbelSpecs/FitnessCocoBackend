using InsightCore.Application.DTO.Tickets;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Tickets.Commands.UpdateTicketStatusCommand
{
    public record UpdateTicketStatusCommand(
        long TicketId,
        bool IsAddressed,
        string? ResolutionNotes) : IRequest<Response<TicketDto>>;
}

