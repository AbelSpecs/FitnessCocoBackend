using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Payments.Commands.CreatePaymentCommand
{
    /// <summary>
    /// Maneja el registro de un nuevo pago en el sistema.
    /// El estado inicial siempre es Pendiente; un coach/admin
    /// lo aprueba o rechaza posteriormente mediante UpdatePaymentCommand.
    /// </summary>
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Response<PaymentDto>>
    {
        private readonly IPaymentsRepository _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePaymentCommandHandler(IPaymentsRepository unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<PaymentDto>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = _mapper.Map<Payment>(request.Payment);
                payment.CreatedAt = DateTime.UtcNow;

                var created = await _unitOfWork.InsertAsync(payment);
                var dto = _mapper.Map<PaymentDto>(created);

                return new Response<PaymentDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = "Pago registrado correctamente. Estado: Pendiente de verificación."
                };
            }
            catch (Exception ex)
            {
                return new Response<PaymentDto> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
