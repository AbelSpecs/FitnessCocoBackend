using AutoMapper;
using InsightCore.Application.DTO.Tickets;
using InsightCore.Persistence.Contexts;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InsightCore.Application.UseCases.Tickets.Queries.GetMyTicketsQuery
{
    public class GetMyTicketsQueryHandler : IRequestHandler<GetMyTicketsQuery, Response<List<TicketDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMyTicketsQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<TicketDto>>> Handle(GetMyTicketsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<TicketDto>>();

            try
            {
                var tickets = await _context.Tickets
                    .AsNoTracking()
                    .Where(t => t.UserId == request.UserId)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync(cancellationToken);

                response.Data = _mapper.Map<List<TicketDto>>(tickets);
                response.IsSuccess = true;
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
