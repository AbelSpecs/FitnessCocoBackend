using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Payments.Queries.GetPaymentByIdQuery
{
    /// <summary>
    /// Maneja la consulta de un pago específico por su Id.
    /// </summary>
    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Response<PaymentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPaymentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<PaymentDto>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await _unitOfWork.Payments.GetByIdAsync(request.Id);
                if (payment == null)
                {
                    return new Response<PaymentDto>
                    {
                        IsSuccess = false,
                        Message = $"No se encontró el pago con Id {request.Id}."
                    };
                }

                var dto = _mapper.Map<PaymentDto>(payment);
                return new Response<PaymentDto> { IsSuccess = true, Data = dto };
            }
            catch (Exception ex)
            {
                return new Response<PaymentDto> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
