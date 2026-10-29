using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Payments.Commands.UpdatePaymentCommand
{
    /// <summary>
    /// Command para modificar los datos o el estado de un pago existente.
    /// Se usa principalmente para aprobar o rechazar un pago pendiente.
    /// </summary>
    public class UpdatePaymentCommand : IRequest<Response<PaymentDto>>
    {
        /// <summary>Id del pago a actualizar.</summary>
        public int Id { get; set; }

        public CreateOrUpdatePaymentDto Payment { get; set; } = null!;
    }
}
