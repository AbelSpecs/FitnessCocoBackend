using AutoMapper;
using InsightCore.Application.DTO.Tickets;
using InsightCore.Persistence.Contexts;
using InsightCore.Transversal.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InsightCore.Application.UseCases.Tickets.Queries.GetAllTicketsQuery
{
    public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, Response<List<TicketDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllTicketsQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<TicketDto>>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<TicketDto>>();

            try
            {
                var query = _context.Tickets
                    .AsNoTracking()
                    .Include(t => t.User)
                    .AsQueryable();

                if (request.IsAddressed.HasValue)
                {
                    query = query.Where(t => t.IsAddressed == request.IsAddressed.Value);
                }

                var tickets = await query
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync(cancellationToken);

                var dtos = tickets.Select(t =>
                {
                    var dto = _mapper.Map<TicketDto>(t);
                    dto.ReporterName = t.User != null ? $"{t.User.FirstName} {t.User.LastName}".Trim() : null;
                    return dto;
                }).ToList();

                response.Data = dtos;
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
