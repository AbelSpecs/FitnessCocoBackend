using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Payments.Commands.CreatePaymentCommand
{
    /// <summary>
    /// Command para registrar un nuevo pago (Bs, USD o USDT)
    /// con su referencia bancaria o hash de transacción.
    /// </summary>
    public class CreatePaymentCommand : IRequest<Response<PaymentDto>>
    {
        public CreateOrUpdatePaymentDto Payment { get; set; } = null!;
    }
}
