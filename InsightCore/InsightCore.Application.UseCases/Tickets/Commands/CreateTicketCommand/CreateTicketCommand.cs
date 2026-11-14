using InsightCore.Application.DTO.Tickets;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Tickets.Commands.CreateTicketCommand
{
    public record CreateTicketCommand(
        int UserId,
        string BugName,
        string Description,
        string Platform,
        string? AppVersion,
        string? DeviceModel) : IRequest<Response<TicketDto>>;
}

