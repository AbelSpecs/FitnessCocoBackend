using AutoMapper;
using InsightCore.Application.DTO.Tickets;
using InsightCore.Persistence.Contexts;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InsightCore.Application.UseCases.Tickets.Commands.UpdateTicketStatusCommand
{
    public class UpdateTicketStatusCommandHandler : IRequestHandler<UpdateTicketStatusCommand, Response<TicketDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateTicketStatusCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<TicketDto>> Handle(UpdateTicketStatusCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<TicketDto>();

            try
            {
                var ticket = await _context.Tickets
                    .FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken);

                if (ticket == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Ticket no encontrado.";
                    return response;
                }

                ticket.IsAddressed = request.IsAddressed;
                ticket.ResolutionNotes = request.ResolutionNotes;
                ticket.AddressedAt = request.IsAddressed
                    ? (ticket.AddressedAt ?? DateTime.UtcNow)
                    : null;

                await _context.SaveChangesAsync(cancellationToken);

                response.Data = _mapper.Map<TicketDto>(ticket);
                response.IsSuccess = true;
                response.Message = "Estado del ticket actualizado.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return response;
        }
    }
}
