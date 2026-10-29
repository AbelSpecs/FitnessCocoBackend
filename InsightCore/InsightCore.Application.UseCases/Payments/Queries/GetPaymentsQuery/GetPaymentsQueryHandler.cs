using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System.Linq;

namespace InsightCore.Application.UseCases.Payments.Queries.GetPaymentsQuery
{
    /// <summary>
    /// Maneja la consulta paginada de pagos, aplicando filtros opcionales
    /// (usuario, alumno, moneda, estado, rango de fechas).
    /// </summary>
    public class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, Response<PagedResult<PaymentDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPaymentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<PagedResult<PaymentDto>>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Normalizar PageSize para evitar consultas excesivamente grandes
                var pageSize = Math.Min(request.PageSize, 100);

                var (items, totalCount) = await _unitOfWork.Payments.GetFilteredAsync(
                    userId: request.UserId,
                    studentId: request.StudentId,
                    currency: request.Currency,
                    status: request.Status,
                    from: request.From,
                    to: request.To,
                    page: request.Page,
                    pageSize: pageSize);

                var dtos = items.Select(p => _mapper.Map<PaymentDto>(p));
                var pagedResult = new PagedResult<PaymentDto>(dtos, totalCount, request.Page, pageSize);

                return new Response<PagedResult<PaymentDto>> { IsSuccess = true, Data = pagedResult };
            }
            catch (Exception ex)
            {
                return new Response<PagedResult<PaymentDto>> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
