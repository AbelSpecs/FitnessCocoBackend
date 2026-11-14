using AutoMapper;
using InsightCore.Application.DTO.Tickets;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Tickets.Commands.CreateTicketCommand
{
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Response<TicketDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTicketCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<TicketDto>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<TicketDto>();

            try
            {
                var ticket = new Ticket
                {
                    UserId = request.UserId,
                    BugName = request.BugName.Trim(),
                    Description = request.Description.Trim(),
                    Platform = string.IsNullOrWhiteSpace(request.Platform)
                        ? "android"
                        : request.Platform.Trim().ToLowerInvariant(),
                    AppVersion = request.AppVersion,
                    DeviceModel = request.DeviceModel,
                    IsAddressed = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync(cancellationToken);

                response.Data = _mapper.Map<TicketDto>(ticket);
                response.IsSuccess = true;
                response.Message = "Ticket de bug reportado exitosamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
